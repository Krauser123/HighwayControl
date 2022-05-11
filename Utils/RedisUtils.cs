using StackExchange.Redis;
using System.Net;

namespace Utils
{
    public class RedisUtils
    {
        private ConnectionMultiplexer Connection;
        private IDatabase Database;
        private EndPoint EndPoint;

        public RedisUtils(string connStrg)
        {
            ConfigurationOptions options = ConfigurationOptions.Parse(connStrg);

            Connection = ConnectionMultiplexer.Connect(options);
            EndPoint = Connection.GetEndPoints().First();
            Database = Connection.GetDatabase();
        }

        public RedisKey[] GetKeysStartWith(string startsWith)
        {
            var pattern = $"{startsWith}:*";
            RedisKey[] keys = Connection.GetServer(EndPoint).Keys(pattern: pattern).ToArray();
            return keys;
        }

        public async Task<string> GetStringAsync(string key)
        {
            string keyValue = await Database.StringGetAsync(key);
            return keyValue;
        }
    }
}
