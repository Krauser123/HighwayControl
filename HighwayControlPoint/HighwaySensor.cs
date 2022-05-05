using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;
using Utils;

namespace HighwayControlPoint
{
    internal class HighwaySensor
    {
        public int Id { get; set; }
        public string Name { get; set; }

        private readonly Random Random;

        private readonly IDatabase DbCache;

        private readonly ILogger logger;


        public HighwaySensor(int id, string name, IDatabase distributedCache, ILogger logger)
        {
            this.Id = id;
            this.Name = name;
            this.Random = new Random();
            this.DbCache = distributedCache;
            this.logger = logger;
        }

        public void StartSensor()
        {
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();

            for (int i = 0; i < 4; i++)
            {
                int carSpeed = Random.Next(10, 140);
                var carPlate = GeneratePlates();

                //Create sensor data
                var sensorData = new SensorData(carSpeed, carPlate, DateTime.Now.ToUniversalTime());

                //Send data to redis cache
                string dataToSave = JsonConvert.SerializeObject(sensorData);
                DbCache.StringSetAsync(Id.ToString(), dataToSave);

                Thread.Sleep(0);
            }

            //Main thread: Call Join(), to wait until ThreadProc ends
            t.Join();

            //Main thread: ThreadProc.Join has returned
        }

        private void ThreadProc()
        {
            for (int i = 0; i < 10; i++)
            {
                logger.LogInformation("ThreadProc: {0}", i);
                Thread.Sleep(0);
            }
        }

        private string GeneratePlates()
        {
            string[] licenseLetters = { "a", "b", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };

            // Generate random number and fit template using the ToString method.
            string template = "0000";
            int value = Random.Next(0, 9999);
            string plate = value.ToString(template);

            //Get 3 random letters
            var partialChars = string.Empty;
            for (int times = 0; times < 3; times++)
            {
                partialChars += licenseLetters[Random.Next(0, licenseLetters.Length)];
            }

            //Concat number and letters
            plate = plate + " " + partialChars.ToUpper();

            return plate;
        }

    }
}
