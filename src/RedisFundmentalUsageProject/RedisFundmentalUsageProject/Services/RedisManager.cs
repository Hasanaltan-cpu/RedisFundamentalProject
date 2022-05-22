using RedisFundmentalUsageProject.Data;
using StackExchange.Redis;

namespace RedisFundmentalUsageProject.Services
{
    public class RedisManager : IRedisManager
    {
        private readonly RedisConnectionFactory _redisConnectionFactory;


        public RedisManager(RedisConnectionFactory redisConnectionFactory)
        {
            _redisConnectionFactory = redisConnectionFactory ?? throw new ArgumentNullException(nameof(redisConnectionFactory));

            TestRedisDatabase = _redisConnectionFactory.GetNotificationHubRedis().GetDatabase();
            TestRedisServer = _redisConnectionFactory.notificationHubServer;
        }

        public IDatabase TestRedisDatabase { get; }
        public IServer TestRedisServer { get; }
    }
}
