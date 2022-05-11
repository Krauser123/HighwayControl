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
        public int Km { get; set; }
        private readonly Random Random;
        private readonly IDatabase DbCache;
        private readonly ILogger Logger;
        private readonly int SensorLive = 40000;


        public HighwaySensor(int id, string name, int km, IDatabase distributedCache, ILogger logger)
        {
            this.Id = id;
            this.Name = name;
            this.Km = km;
            this.Random = new Random();
            this.DbCache = distributedCache;
            this.Logger = logger;
            InsertSensorInRedis();
        }

        public void StartSensor()
        {
            Thread t = new Thread(new ThreadStart(ThreadProc));
            t.Start();

            for (int i = 0; i < SensorLive; i++)
            {
                int carSpeed = Random.Next(10, 140);
                var carPlate = GeneratePlates();

                //Create sensor data
                var sensorData = new SensorData(carSpeed, carPlate, DateTime.Now.ToUniversalTime());

                //Send data to redis cache
                string dataToSave = JsonConvert.SerializeObject(sensorData);

                //Build key for redis
                string key = $"{Id}:{sensorData.CatchDate.ToShortDateString()}:{sensorData.CatchDate.GetHashCode()}";

                DbCache.StringSetAsync(key, dataToSave);
                Logger.LogTrace($"Data sended to redis: SensorInfo (Id: {Id} Name: {Name} Km: {Km}) SensorData (Plate: {sensorData.CarPlate} SpeedcarSpeed: {sensorData.CarSpeed}) ");

                Thread.Sleep(0);
            }

            //Main thread: Call Join(), to wait until ThreadProc ends
            t.Join();

            //Main thread: ThreadProc.Join has returned
        }

        private void InsertSensorInRedis()
        {
            var exist = DbCache.StringGet($"Sensors:{this.Id}");

            if (!exist.HasValue || exist != this.Name)
            {
                DbCache.StringSetAsync($"Sensors:{this.Id}", $"{this.Name}");
            }
        }

        private void ThreadProc()
        {
            for (int i = 0; i < 10; i++)
            {
                Logger.LogInformation("ThreadProc: {0}", i);
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
