using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utils;

namespace HighwayControlPoint
{
    internal class Program
    {
        // AutoResetEvent to signal when to exit the application.
        private static readonly AutoResetEvent waitHandle = new AutoResetEvent(false);

        static void Main(string[] args)
        {
            // Fire and forget
            Task.Run(() =>
                {
                    while (true)
                    {
                        
                    }
                });

            SensorsSetup sensorsSetup = new SensorsSetup(args);
            sensorsSetup.InitializeSensors();

            // Handle Control+C or Control+Break
            Console.CancelKeyPress += (o, e) =>
            {
                Console.WriteLine("Exit");

                // Allow the manin thread to continue and exit...
                waitHandle.Set();
            };

            // Wait
            waitHandle.WaitOne();
        }
    }

    internal class SensorsSetup
    {
        IConfigurationRoot ConfigurationRoot;
        string RedisConnection;
        ILogger<Program> Logger;
        List<SensorCreateInfo> SensorsToInitialize = new List<SensorCreateInfo>();

        public SensorsSetup(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Information)
                       .AddFilter("System", LogLevel.Information)
                       .AddFilter("Default", LogLevel.Information)
                       .AddConsole();
            });

            Logger = loggerFactory.CreateLogger<Program>();

            using IHost host = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                //Get environment
                IHostEnvironment env = hostingContext.HostingEnvironment;

                //Check if we received appSettings through args
                string appSettingsName = "appsettings.json";
                if (args != null && args.Length > 0 && File.Exists(args[0]))
                {
                    appSettingsName = args[0];
                    Logger.LogInformation($"Using {args[0]} setup.");
                }
                else
                {
                    Logger.LogWarning($"Warning: Specific appSettings not found. Getting {appSettingsName} instead.");
                }

                //Add specific json setup
                configuration.AddJsonFile(appSettingsName, optional: true, reloadOnChange: true);

                //Add specific environment vars (This is utils if we want to set some properties directly in azure and don't have secrets in local appSettings)
                configuration.AddEnvironmentVariables();

                ConfigurationRoot = configuration.Build();
            }).Build();

            //Get sensors to initialize
            SensorsToInitialize = ConfigurationRoot.GetSection(Constants.SensorsToInitialize).Get<List<SensorCreateInfo>>();

            //Get connections to Redis
            RedisConnection = ConfigurationRoot.GetValue<string>(Constants.RedisConnection);
        }

        public void InitializeSensors()
        {
            //Start a new controlPoint
            var controlPoint = new ControlPoint(SensorsToInitialize, RedisConnection, Logger);
            controlPoint.Start();
        }
    }
}