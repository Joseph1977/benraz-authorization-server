using Authorization.Application.Notifications;
using Authorization.Domain;
using Authorization.Domain.Applications;
using Authorization.Domain.ApplicationTokens;
using Authorization.Domain.Claims;
using Authorization.Domain.Emails;
using Authorization.Domain.Roles;
using Authorization.Domain.SsoConnections;
using Authorization.Domain.SsoServices;
using Authorization.Domain.UsageLogs;
using Authorization.Domain.Users;
using Authorization.EF;
using Authorization.EF.Repositories;
using Authorization.EF.Services;
using Authorization.Infrastructure.Gateways.FacebookGraph;
using Authorization.Infrastructure.Gateways.MicrosoftGraph;
using Authorization.Infrastructure.Jwt;
using Authorization.WebApi.Authorization;
using Authorization.WebApi.Controllers;
using Authorization.WebApi.Extensions;
using Authorization.WebApi.Filtes;
using Authorization.WebApi.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Benraz.Infrastructure.Authorization.Tokens;
using Benraz.Infrastructure.Common.AccessControl;
using Benraz.Infrastructure.Common.DataRedundancy;
using Benraz.Infrastructure.Emails;
using Benraz.Infrastructure.Gateways.BenrazAuthorization.Auth;
using Benraz.Infrastructure.Gateways.BenrazServices;
using Benraz.Infrastructure.Web.Authorization;
using Benraz.Infrastructure.Web.Filters;
using Asp.Versioning.ApiExplorer;
using System;
using System.Reflection;
using IEmailsService = Authorization.Domain.Emails.IEmailsService;
using Asp.Versioning;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;


namespace Authorization.WebApi
{
    /// <summary>
    /// Application startup.
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
            if (IsEnablesApplicationInsights())
            {
                services.AddApplicationInsightsTelemetry();
            }

            services.AddMvc(config =>
            {
                config.Filters.Add<ErrorFilterAttribute>();
                config.Filters.Add<LoggingFilterAttribute>();
            });
            services.AddCors();
            services.AddHttpContextAccessor();

            ConfigureSqlServerContext(services);

            services
                .AddIdentityCore<User>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequiredLength = 8;
                    options.Lockout.MaxFailedAccessAttempts = int.MaxValue; // managed by custom service
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<AuthorizationDbContext>()
                .AddDefaultTokenProviders();

            services
                .AddSingleton(provider =>
                {
                    var maskingDataService = provider.GetService<IMaskingDataService>();

                    var mapperConfig = new MapperConfiguration(config =>
                    {
                        if (maskingDataService != null)
                        {
                            config.AddProfile(new AutoMapperProfile(maskingDataService));
                        }
                        else
                        {
                            // Provide a fallback behavior or default profile
                            config.AddProfile(new DefaultAutoMapperProfile());
                        }
                    });

                    return mapperConfig.CreateMapper();
                });

            services.AddSingleton(provider =>
            {
                var maskingDataService = provider.GetService<IMaskingDataService>();
                // Check if the maskingDataService is null
                if (maskingDataService == null)
                {
                    throw new InvalidOperationException("IMaskingDataService is not registered in the DI container.");
                }
                var mapperConfig = new MapperConfiguration(config =>
                                    {
                                        config.AddProfile(new AutoMapperProfile(maskingDataService));
                                    });

                return mapperConfig.CreateMapper();
            });

            services.AddControllers()
                .AddApplicationPart(Assembly.GetAssembly(typeof(ITController)) ?? throw new InvalidOperationException("Assembly containing ITController could not be found."));


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
                var allowedHosts = Configuration.GetValue<string>("AllowedHosts");

                if (!string.IsNullOrEmpty(allowedHosts))
                {
                    options
                        .WithOrigins(allowedHosts)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
                else
                {
                    // Handle the case where AllowedHosts is null or empty
                    // You can either use AllowAnyOrigin or throw an exception, depending on your requirements
                    options
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            });

            // To be able to get the IP adderss by: Request.HttpContext.Connection.RemoteIpAddress
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
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

            UseDatabaseMigrationsAsync(app).GetAwaiter().GetResult();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwagger(apiVersionDescriptionProvider, Configuration);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private bool IsEnablesApplicationInsights()
        {
            return Configuration.GetValue<bool>("EnablesApplicationInsights");
        }

        private void ConfigureSqlServerContext(IServiceCollection services)
        {
            string? connectionString = Configuration.GetConnectionString("Authorization");
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
            services.AddHttpClient();

            services.AddSingleton<IDrChecker, DrChecker>();
            services.AddScoped<DRFilterAttribute>();

            services.AddTransient<IUsageLogsRepository, UsageLogsRepository>();
            services.AddTransient<LogUsageFilter>();

            services.AddScoped<IPrincipalService, HttpContextPrincipalService>();

            services.AddTransient<IMigrationService, AuthorizationDbMigrationService>();

            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.Configure<AuthorizationServiceSettings>(Configuration.GetSection("Authorization"));

            services.AddTransient<IApplicationsRepository, ApplicationsRepository>();

            services.AddTransient<IApplicationTokensService, ApplicationTokensService>();
            services.AddTransient<IApplicationTokensRepository, ApplicationTokensRepository>();

            services.AddTransient<ISsoConnectionsRepository, SsoConnectionsRepository>();
            services.AddTransient<ISsoConnectionsService, SsoConnectionsService>();

            services.AddTransient<IRolesRepository, RolesRepository>();
            services.AddTransient<IClaimsRepository, ClaimsRepository>();

            services.AddTransient<IUsersRepository, UsersRepository>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<IUserPasswordsRepository, UserPasswordsRepository>();
            services.AddTransient<IUserPasswordsService, UserPasswordsService>();
            services.Configure<UserPasswordsServiceSettings>(Configuration.GetSection("UserPasswords"));

            services.AddTransient<ISsoServiceProvider, SsoServiceProvider>();
            services.AddTransient<IJwtService, JwtService>();
            services.Configure<JwtServiceSettings>(Configuration.GetSection("Jwt"));

            services.AddTransient<IInternalSsoService, InternalSsoService>();
            services.AddTransient<InternalSsoService>();
            services.Configure<InternalSsoServiceSettings>(Configuration.GetSection("InternalSsoService"));

            services.AddTransient<MicrosoftSsoService>();

            services.AddTransient<FacebookSsoService>();
            services.Configure<FacebookSsoServiceSettings>(Configuration.GetSection("FacebookSsoService"));

            services.AddTransient<GoogleSsoService>();
            services.Configure<GoogleSsoServiceSettings>(Configuration.GetSection("GoogleSsoService"));

            services.AddTransient<IAccessTokenAuthenticationService, AccessTokenAuthenticationService>();

            services.AddTransient<IEmailsService, EmailsService>();
            services.Configure<EmailsServiceSettings>(Configuration.GetSection("Emails"));

            services.AddTransient<IEmailsServiceProvider, EmailsServiceProvider>();
            services.AddTransient<IEmailsNotificationFactory, EmailsNotificationFactory>();
            services.Configure<EmailsServiceProviderSettings>(Configuration.GetSection("Emails:ServiceProvider"));

            services.AddTransient<IMicrosoftGraphGateway, MicrosoftGraphGateway>();
            services.Configure<MicrosoftGraphGatewaySettings>(Configuration.GetSection("MicrosoftGraph"));

            services.AddTransient<IFacebookGraphGateway, FacebookGraphGateway>();
            services.Configure<FacebookGraphGatewaySettings>(Configuration.GetSection("FacebookGraphGateway"));

            services.AddTransient<IBenrazServicesGateway, BenrazServicesGateway>();

            services.AddTransient<IMaskingDataService, MaskingDataService>();
            services.AddTransient<IUsageLogsService, UsageLogsService>();

            services.Configure<UserServiceSettings>(Configuration.GetSection("User"));

            services.Configure<UserActionNotificationsServiceSettings>(Configuration.GetSection("UserActionNotifications"));

            services.AddTransient<IUserMfasRepository, UserMfasRepository>();
        }

        /// <summary>
        /// Adds authorization.
        /// </summary>
        /// <param name="services">Services.</param>
        protected virtual void AddAuthorization(IServiceCollection services)
        {
            services.Configure<BenrazAuthorizationAuthGatewaySettings>(Configuration.GetSection("BenrazAuthGateway"));
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

                    // Switch to using TokenHandlers
                    options.TokenHandlers.Clear();  // Clear existing handlers if any
                    options.TokenHandlers.Add(new JwtSecurityTokenHandler());  // Add JwtSecurityTokenHandler for handling JWTs

                    // Configure the TokenValidationParameters to specify how tokens should be validated
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(5),
                        IssuerSigningKeyResolver = tokenValidationService.IssuerSigningKeyResolver,
                        ValidateAudience = true,
                        AudienceValidator = tokenValidationService.AudienceValidator,
                        ValidateIssuer = true,
                        IssuerValidator = tokenValidationService.IssuerValidator,

                        // Include additional validation settings as needed
                    };

                    options.Audience = Configuration["Jwt:Audience"];
                    options.Authority = Configuration["Jwt:Authority"];

                });

            services
                .AddAuthorization(options =>
                {
                    options.AddClaimsPolicy(ApplicationPolicies.APPLICATION_READ, ApplicationClaims.APPLICATION_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.APPLICATION_ADD, ApplicationClaims.APPLICATION_ADD);
                    options.AddClaimsPolicy(ApplicationPolicies.APPLICATION_UPDATE,
                        ApplicationClaims.APPLICATION_UPDATE);
                    options.AddClaimsPolicy(ApplicationPolicies.APPLICATION_DELETE,
                        ApplicationClaims.APPLICATION_DELETE);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_READ, ApplicationClaims.USER_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_ADD, ApplicationClaims.USER_ADD);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_UPDATE, ApplicationClaims.USER_UPDATE);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_DELETE, ApplicationClaims.USER_DELETE);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_STATUS_READ, ApplicationClaims.USER_STATUS_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_STATUS_SUSPEND,
                        ApplicationClaims.USER_STATUS_SUSPEND);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_STATUS_BLOCK, ApplicationClaims.USER_STATUS_BLOCK);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_UNLOCK, ApplicationClaims.USER_UNLOCK);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_ROLE_READ, ApplicationClaims.USER_ROLE_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_ROLE_UPDATE, ApplicationClaims.USER_ROLE_UPDATE);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_CLAIM_READ, ApplicationClaims.USER_CLAIM_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_CLAIM_UPDATE, ApplicationClaims.USER_CLAIM_UPDATE);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_EMAIL_READ, ApplicationClaims.USER_EMAIL_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_EMAIL_UPDATE, ApplicationClaims.USER_EMAIL_UPDATE);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_EMAIL_VERIFY, ApplicationClaims.USER_EMAIL_VERIFY);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_PHONE_READ, ApplicationClaims.USER_PHONE_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_PHONE_VERIFY, ApplicationClaims.USER_PHONE_VERIFY);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_PASSWORD_UPDATE,
                        ApplicationClaims.USER_PASSWORD_UPDATE);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_PASSWORD_RESET,
                        ApplicationClaims.USER_PASSWORD_RESET);
                    options.AddClaimsPolicy(ApplicationPolicies.ROLE_READ, ApplicationClaims.ROLE_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.ROLE_ADD, ApplicationClaims.ROLE_ADD);
                    options.AddClaimsPolicy(ApplicationPolicies.ROLE_UPDATE, ApplicationClaims.ROLE_UPDATE);
                    options.AddClaimsPolicy(ApplicationPolicies.ROLE_DELETE, ApplicationClaims.ROLE_DELETE);
                    options.AddClaimsPolicy(ApplicationPolicies.CLAIM_READ, ApplicationClaims.CLAIM_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.CLAIM_ADD, ApplicationClaims.CLAIM_ADD);
                    options.AddClaimsPolicy(ApplicationPolicies.CLAIM_DELETE, ApplicationClaims.CLAIM_DELETE);
                    options.AddPolicy(
                        ApplicationPolicies.INTERNAL_LOGIN_CHANGE_PASSWORD,
                        builder =>
                        {
                            builder.RequireAssertion(context =>
                                context.User.HasClaim(CustomClaimTypes.IS_PASSWORD_EXPIRED, true.ToString()) ||
                                context.User.HasClaim(CommonClaimTypes.CLAIM,
                                    ApplicationClaims.PROFILE_PASSWORD_CHANGE));
                        });
                    options.AddClaimsPolicy(ApplicationPolicies.INTERNAL_LOGIN_SET_PASSWORD,
                        ApplicationClaims.PROFILE_PASSWORD_SET);
                    options.AddClaimsPolicy(ApplicationPolicies.USER_READ_ONE, ApplicationClaims.USER_READ_ONE,
                        ApplicationClaims.USER_READ);
                    options.AddClaimsPolicy(ApplicationPolicies.TOKEN_EXCHANGE, ApplicationClaims.TOKEN_EXCHANGE);
                    options.AddClaimsPolicy(ApplicationPolicies.INTERNAL_LOGIN_MFA_TOKEN,
                        ApplicationClaims.PROFILE_MFA_TOKEN);
                });
        }

        private static void AddVersioning(IServiceCollection services)
        {
            var versioningBuilder = services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader(); // Use appropriate version reader
            });

            // Add API Explorer for versioned APIs using the versioning builder
            versioningBuilder.AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }

        private async Task UseDatabaseMigrationsAsync(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationService>();
                await migrationService.MigrateAsync(); // Use await instead of .Wait()
            }
            return;
        }

        private bool IsInjectDbCredentialsToConnectionString()
        {
            return Configuration.GetValue<bool>("InjectDBCredentialFromEnvironment");
        }
    }
}