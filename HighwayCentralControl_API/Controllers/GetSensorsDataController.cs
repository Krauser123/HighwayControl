using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Utils;

namespace HighwayCentralControl_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetSensorsDataController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetSensorsDataController> _logger;
        private RedisUtils redisUtils;

        public GetSensorsDataController(ILogger<GetSensorsDataController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            redisUtils = new RedisUtils(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public async Task<List<SensorData>> Get([FromQuery] int sensorId)
        {
            List<SensorData> sensorDatas = new List<SensorData>();

            try
            {
                var keyPrefix = sensorId.ToString() + ":" + DateTime.Now.ToString("dd/MM/yyyy");
                var keys = redisUtils.GetKeysStartWith(keyPrefix);

                _logger.LogInformation($"{keys.Count()} keys found with prefix: {keyPrefix}");

                foreach (var key in keys)
                {
                    var value = await redisUtils.GetStringAsync(key);
                    var singleData = JsonConvert.DeserializeObject<SensorData>(value);
                    sensorDatas.Add(singleData);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error gettings Sensor Data ->" + ex.ToString());
            }

            return sensorDatas;
        }

    }
}