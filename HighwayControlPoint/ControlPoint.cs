using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Utils;

namespace HighwayControlPoint
{
    internal class ControlPoint
    {
        private IDatabase Database;
        private readonly ConnectionMultiplexer Connection;
        private readonly List<HighwaySensor> SensorsInHighway;
        private readonly List<SensorCreateInfo> SensorsToInitialize;
        private readonly ILogger<HighwaySensor> logger;

        public ControlPoint(List<SensorCreateInfo> sensorsInControlPoint, string redisConnectionString)
        {
            var logFactory = new LoggerFactory();
            logger = logFactory.CreateLogger<HighwaySensor>();

            SensorsToInitialize = sensorsInControlPoint;
            SensorsInHighway = new List<HighwaySensor>();
            Connection = GetConnection(redisConnectionString);
        }

        public void Start()
        {
            Database = Connection.GetDatabase();

            foreach (var sensorToInit in SensorsToInitialize)
            {
                var sensor = new HighwaySensor(sensorToInit.Id, sensorToInit.Name, sensorToInit.Km, Database, logger);
                SensorsInHighway.Add(sensor);

                logger.LogInformation($"Starting sensor -> Id: {sensor.Id} - Name: {sensor.Name}");
                sensor.StartSensor();
            }
        }

        private ConnectionMultiplexer GetConnection(string endPoint)
        {
            try
            {
                var connectionMultiplexer = ConnectionMultiplexer.Connect(
                    new ConfigurationOptions
                    {
                        EndPoints = { endPoint }
                    });

                return connectionMultiplexer;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Cannot connect to Redis Database Server. Exception: {ex}");
            }
        }
    }
}
