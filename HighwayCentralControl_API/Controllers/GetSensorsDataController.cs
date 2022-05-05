using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace HighwayCentralControl_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetSensorsDataController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;

        private readonly ILogger<GetSensorsDataController> _logger;

        public GetSensorsDataController(ILogger<GetSensorsDataController> logger, IDistributedCache distributedCache)
        {
            _logger = logger;
            _distributedCache = distributedCache;            
        }

        [HttpGet]
        public string Get([FromQuery] int sensorId)
        {
            string dataToReturn = null;
            
            try
            {
                dataToReturn = _distributedCache.GetString(sensorId.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("Error at Sensor API GET ->" + ex.ToString());
            }

            return dataToReturn;
        }
    }
}