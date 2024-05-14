using Authorization.Domain.Authorization;
using Authorization.Domain.Jobs;
using Authorization.Domain.Settings;
using Authorization.EF;
using Authorization.EF.Repositories;
using Authorization.EF.Services;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Controllers;
using Authorization.WebApi.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Benraz.Infrastructure.Authorization.Tokens;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Common.BackgroundQueue;
using Benraz.Infrastructure.Common.DataRedundancy;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth;
using Benraz.Infrastructure.Web.Authorization;
using Benraz.Infrastructure.Web.Filters;
using System;
using System.Reflection;

namespace Authorization.WebApi
{
    /// <summary>
    /// Startup.
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Creates startup instance.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configures application services.
        /// </summary>
        /// <param name="services">Services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            ConfigureSqlServerContext(services);

            services.AddAutoMapper(typeof(AuthorizationAutoMapperProfile));

            services.AddHttpClient();

            services
                .AddControllers()
                .AddApplicationPart(Assembly.GetAssembly(typeof(ITController)));

            AddVersioning(services);

            services.AddSwagger(Configuration);

            AddServices(services);
            AddAuthorization(services);
        }

        /// <summary>
        /// Configures the application.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <param name="env">Environment.</param>
        /// <param name="apiVersionDescriptionProvider">API version description provider.</param>
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            app.UseCors(options =>
            {
                options
                    .WithOrigins(Configuration.GetValue<string>("AllowedHosts"))
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            UseDatabaseMigrations(app);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwagger(apiVersionDescriptionProvider, Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureSqlServerContext(IServiceCollection services)
        {
            string connectionString = Configuration.GetConnectionString("Authorization");
            if (IsInjectDbCredentialsToConnectionString())
            {
                connectionString +=
                    $";User Id={Environment.GetEnvironmentVariable("ASPNETCORE_DB_USERNAME")};Password={Environment.GetEnvironmentVariable("ASPNETCORE_DB_PASSWORD")}";
            }

            services.AddDbContext<AuthorizationDbContext>(options =>
                options.UseSqlServer(
                    connectionString,
                    o => o.EnableRetryOnFailure(3)
                ));
        }

        private void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IDrChecker, DrChecker>();
            services.AddScoped<DRFilterAttribute>();
            services.AddScoped<ErrorFilterAttribute>();

            services.AddTransient<IDbMigrationService, AuthorizationDbMigrationService>();

            services.AddTransient<ISettingsEntriesRepository, SettingsEntriesRepository>();

            services.AddTransient<IEmptyRepeatableJobsService, EmptyRepeatableJobsService>();

            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
        }

        /// <summary>
        /// Adds authorization.
        /// </summary>
        /// <param name="services">Services.</param>
        protected virtual void AddAuthorization(IServiceCollection services)
        {
            services.Configure<BenrazAuthorizationAuthGatewaySettings>(x =>
            {
                x.BaseUrl = Configuration.GetValue<string>("General:AuthorizationBaseUrl");
            });
            services.AddTransient<IBenrazAuthorizationAuthGateway, BenrazAuthorizationAuthGateway>();

            services.Configure<TokenValidationServiceSettings>(Configuration.GetSection("TokenValidation"));
            services.AddTransient<ITokenValidationService, TokenValidationService>();

            services.AddTransient<TokenValidator>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var serviceProvider = services.BuildServiceProvider();

                    var tokenValidationService = serviceProvider.GetRequiredService<ITokenValidationService>();
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),
                        IssuerSigningKeyResolver = tokenValidationService.IssuerSigningKeyResolver,
                        ValidateAudience = true,
                        AudienceValidator = tokenValidationService.AudienceValidator,
                        ValidateIssuer = true,
                        IssuerValidator = tokenValidationService.IssuerValidator,

                    };

                    var tokenValidator = serviceProvider.GetRequiredService<TokenValidator>();
                    options.SecurityTokenValidators.Clear();
                    options.SecurityTokenValidators.Add(tokenValidator);
                });

            services
                .AddAuthorization(options =>
                {
                    options.AddClaimsPolicy(
                        AuthorizationPolicies.SETTINGS_READ,
                        AuthorizationClaims.SETTINGS_READ);
                    options.AddClaimsPolicy(
                        AuthorizationPolicies.SETTINGS_ADD,
                        AuthorizationClaims.SETTINGS_ADD);
                    options.AddClaimsPolicy(
                        AuthorizationPolicies.SETTINGS_UPDATE,
                        AuthorizationClaims.SETTINGS_UPDATE);
                    options.AddClaimsPolicy(
                        AuthorizationPolicies.SETTINGS_DELETE,
                        AuthorizationClaims.SETTINGS_DELETE);

                    options.AddClaimsPolicy(
                        AuthorizationPolicies.CLAIM_READ, 
                        AuthorizationClaims.CLAIM_READ);
                    options.AddClaimsPolicy(
                        AuthorizationPolicies.CLAIM_ADD, 
                        AuthorizationClaims.CLAIM_ADD);
                    options.AddClaimsPolicy(
                        AuthorizationPolicies.CLAIM_DELETE, 
                        AuthorizationClaims.CLAIM_DELETE);

                    options.AddPolicy(
                        AuthorizationPolicies.JOB_EXECUTE,
                        builder => builder
                            .RequireRole(AuthorizationRoles.INTERNAL_SERVER)
                            .RequireClaim(CommonClaimTypes.CLAIM, AuthorizationClaims.JOB_EXECUTE));

                });
        }

        private static void AddVersioning(IServiceCollection services)
        {
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
        }

        private void UseDatabaseMigrations(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<IDbMigrationService>().MigrateAsync().Wait();
            }
        }

        private bool IsInjectDbCredentialsToConnectionString()
        {
            return Configuration.GetValue<bool>("InjectDBCredentialFromEnvironment");
        }
    }
}

