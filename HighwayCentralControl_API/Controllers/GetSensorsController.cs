using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Utils;

namespace HighwayCentralControl_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetSensorsController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetSensorsDataController> _logger;
        private RedisUtils _redisUtils;

        public GetSensorsController(ILogger<GetSensorsDataController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _redisUtils = new RedisUtils(_configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpGet]
        public async Task<List<SensorCreateInfo>> Get()
        {
            List<SensorCreateInfo> sensors = new List<SensorCreateInfo>();

            try
            {
                var keys = _redisUtils.GetKeysStartWith(Constants.SensorsKey);

                foreach (var key in keys)
                {
                    var sensorId = Convert.ToInt16(key.ToString().Split(":")[1]);
                    var value = await _redisUtils.GetStringAsync(key);
                    sensors.Add(new SensorCreateInfo { Id = sensorId, Name = value });
                }

                sensors = sensors.OrderBy(o => o.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error gettings Sensor Data ->" + ex.ToString());
            }

            return sensors;
        }
    }
}