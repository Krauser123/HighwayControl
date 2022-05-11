using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Utils;

namespace HighwayCentralControl_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetSensorsDataController : ControllerBase
    {
        private readonly IDistributedCache _redis;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GetSensorsDataController> _logger;
        private RedisUtils redisUtils;

        public GetSensorsDataController(ILogger<GetSensorsDataController> logger, IDistributedCache distributedCache, IConfiguration configuration)
        {
            _logger = logger;
            _redis = distributedCache;
            _configuration = configuration;
            redisUtils = new RedisUtils();
        }

        [HttpGet]
        public async Task<string> Get([FromQuery] int sensorId, [FromQuery] DateTime date)
        {
            string dataToReturn = null;

            try
            {
                var keys = redisUtils.GetKeys(sensorId.ToString() + ":" + date.ToShortDateString(), _configuration.GetConnectionString("DefaultConnection"));

                foreach (var key in keys)
                {
                    var value = await _redis.GetStringAsync(key);
                    var valueN = JsonConvert.DeserializeObject<object>(value);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error gettings Sensor Data ->" + ex.ToString());
            }

            return dataToReturn;
        }
       
    }
}