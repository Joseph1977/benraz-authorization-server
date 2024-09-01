using Authorization.WebApi.Configuration;
using Microsoft.AspNetCore;
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
            Logger logger = null;
            try
            {
                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var config = new ConfigurationBuilder()
                    .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
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
                    var connectionString = builder.Build().GetConnectionString("Authorization");
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
            var logFolder = config.GetValue<string>("General:LogFolder");
            InternalLogger.LogFile = Path.Combine(logFolder, "nlog-internal.log");

            var logFactory = NLogBuilder.ConfigureNLog("nlog.config");
            logFactory.Configuration.Variables["logFolder"] = logFolder;

            var logger = logFactory.GetCurrentClassLogger();
            logger.Info("Starting application...");

            return logger;
        }
    }
}


