using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace HighwayControlPoint
{
    internal class ControlPoint
    {
        private IDatabase Database;
        private readonly ConnectionMultiplexer Connection;
        private readonly List<HighwaySensor> SensorsInHighway;
        private readonly Dictionary<int, string> SensorsToInitialize;
        private readonly ILogger<HighwaySensor> logger; 

        public ControlPoint(Dictionary<int, string> sensorsInControlPoint)
        {
            var logFactory = new LoggerFactory();
            logger = logFactory.CreateLogger<HighwaySensor>();

            SensorsToInitialize = sensorsInControlPoint;
            SensorsInHighway = new List<HighwaySensor>();
            Connection = GetConnection("localhost:6379");
        }

        public void Start()
        {
            Database = Connection.GetDatabase();

            foreach (var sensorToInit in SensorsToInitialize)
            {
                var sensor = new HighwaySensor(sensorToInit.Key, sensorToInit.Value, Database, logger);
                SensorsInHighway.Add(sensor);

                Console.WriteLine("Starting sensor -> Id {0} - Name {1} ", sensor.Id, sensor.Name);
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
                throw new ApplicationException("$Cannot connect to Redis Database Server. Exception {0}", ex);
            }
        }
    }
}
