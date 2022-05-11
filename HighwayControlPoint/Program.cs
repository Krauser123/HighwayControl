using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Utils;

namespace HighwayControlPoint
{
    internal class Program
    {
        static IConfigurationRoot ConfigurationRoot;
        static List<SensorCreateInfo> SensorsToInitialize = new List<SensorCreateInfo>();
        static string RedisConnection;

        static void Main(string[] args)
        {
            var logFactory = new LoggerFactory();
            var logger = logFactory.CreateLogger<Program>();

            using IHost host = Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((hostingContext, configuration) =>
            {
                configuration.Sources.Clear();
                IHostEnvironment env = hostingContext.HostingEnvironment;

                //Check if we received appSettings through args
                string appSettingsName = "appsettings.json";
                if (args != null && args.Length > 0 && File.Exists(args[0]))
                {
                    appSettingsName = args[0];
                }
                else
                {
                    logger.LogWarning($"Warning: Specific appSettings not found. Getting {appSettingsName} instead.");
                }

                configuration.AddJsonFile(appSettingsName, optional: true, reloadOnChange: true);

                //Add specific environment vars (This is utils if we want to set some properties directly in azure and don't have secrets in local appSettings)
                configuration.AddEnvironmentVariables();

                ConfigurationRoot = configuration.Build();
            }).Build();

            //Get sensors to initialize
            SensorsToInitialize = ConfigurationRoot.GetSection(Constants.SensorsToInitialize).Get<List<SensorCreateInfo>>();

            //Get connections to Redis
            RedisConnection = ConfigurationRoot.GetValue<string>(Constants.RedisConnection);

            //Start a new controlPoint
            var controlPoint = new ControlPoint(SensorsToInitialize, RedisConnection);
            controlPoint.Start();

            Console.ReadLine();
        }
    }
}