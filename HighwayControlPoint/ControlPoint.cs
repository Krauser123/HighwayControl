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
        private readonly ILogger Logger;

        public ControlPoint(List<SensorCreateInfo> sensorsInControlPoint, string redisConnectionString, ILogger log)
        {
            Logger = log;
            SensorsToInitialize = sensorsInControlPoint;
            SensorsInHighway = new List<HighwaySensor>();
            Connection = GetConnection(redisConnectionString);
            Database = Connection.GetDatabase();
        }

        public void Start()
        {
            foreach (var sensorToInit in SensorsToInitialize)
            {
                var sensor = new HighwaySensor(sensorToInit.Id, sensorToInit.Name, sensorToInit.Km, Database, Logger);
                SensorsInHighway.Add(sensor);

                Logger.LogInformation($"Starting sensor -> Id: {sensor.Id} - Name: {sensor.Name}");
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
