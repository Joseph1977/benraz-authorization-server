using Authorization.WebApi.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Common;
using NLog.Web;
using System;
using System.IO;
using System.Reflection;

namespace Authorization.WebApi
{
    /// <summary>
    /// Program.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main entry point.
        /// </summary>
        /// <param name="args">Arguments.</param>
        public static void Main(string[] args)
        {
            Logger? logger = null;
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory();


                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                    .Build();

                logger = InitLogger(config);

                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                logger?.Error(ex, "Fatal error occured, stopping application...");
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        /// <summary>
        /// Creates and returns web host builder instance.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <returns>Web host builder.</returns>
        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(builder =>
                {
                    var connectionString = GetSqlServerConnectionString(builder);
                    builder.AddDatabaseConfiguration(options => options.UseSqlServer(connectionString));
                })
                .UseStartup<Startup>()
                .ConfigureLogging(logging => logging.ClearProviders())
                .UseNLog(new NLogAspNetCoreOptions
                {
                    CaptureMessageProperties = true,
                    CaptureMessageTemplates = true
                });
        }

        private static Logger InitLogger(IConfigurationRoot config)
        {
            string logFolder = config.GetValue<string>("General:LogFolder") ?? ".\\"; // throw new ArgumentNullException(nameof(logFolder));
            InternalLogger.LogFile = Path.Combine(logFolder, "nlog-internal.log");

            // Use the new recommended setup method
            var logFactory = NLog.LogManager.Setup()
                .LoadConfigurationFromFile("nlog.config") // Load configuration from nlog.config file
                .LoadConfiguration(configBuilder =>
                {
                    var loggingConfiguration = configBuilder.Configuration;
                    loggingConfiguration.Variables["logFolder"] = logFolder;
                });

            var logger = NLog.LogManager.GetCurrentClassLogger(); // Get the logger after setting up

            logger.Info("Starting application...");

            return logger;
        }

        private static string? GetSqlServerConnectionString(IConfigurationBuilder builder)
        {
            var connectionString = builder.Build().GetConnectionString("Authorization");
            var isInjectDbCredentialsToConnectionString =
                builder.Build().GetValue<bool>("InjectDBCredentialFromEnvironment");
            if (!isInjectDbCredentialsToConnectionString) 
                return connectionString;

            var userName = Environment.GetEnvironmentVariable("ASPNETCORE_DB_USERNAME");
            var password = Environment.GetEnvironmentVariable("ASPNETCORE_DB_PASSWORD");
            connectionString +=
                $";User Id={userName};Password={password}";

            return connectionString;
        }
    }
}


