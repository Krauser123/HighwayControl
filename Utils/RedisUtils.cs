using StackExchange.Redis;

namespace Utils
{
    public class RedisUtils
    {
        public RedisKey[] GetKeys(string name, string connStrg)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(connStrg);
            ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
            var endPoint = connection.GetEndPoints().First();
            var pattern = $"{name}:*";
            RedisKey[] keys = connection.GetServer(endPoint).Keys(pattern: pattern).ToArray();

            /*var db= connection.GetDatabase();
            foreach (var key in keys)
            {
                var as2=db.StringGet(key);
            }*/

            return keys;
        }
    }
}
