using StackExchange.Redis;

namespace RedisFundmentalUsageProject.Data
{

    public class RedisConnectionFactory
    {
        private Lazy<ConnectionMultiplexer> _testRedis;
        private int _currentDatabaseId = 0;

        private readonly IConfiguration _configuration;

        public RedisConnectionFactory(IConfiguration configuration )
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _testRedis = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(GetNotificationHubRedisConnectionOptions(_configuration)));
        }
        public IServer notificationHubServer
        {
            get
            {
                string redisHost = "127.0.0.1";
                int redisPort = 6379;

                return _testRedis.Value.GetServer(redisHost, redisPort);
            }
        }

        private static ConfigurationOptions GetNotificationHubRedisConnectionOptions(IConfiguration configuration)
        {
            var redisConfig = "127.0.0.1,allowAdmin=true";

            ConfigurationOptions options = ConfigurationOptions.Parse(redisConfig);
            options.ClientName = "RCServiceOldMapCaching";
            options.ConnectRetry = 3;
            options.ConnectTimeout = 100000;
            options.KeepAlive = 180;
            options.ResolveDns = false;
            options.SyncTimeout = 100000;
            options.AbortOnConnectFail = false;
            //options.Password = ; if u use password


            return options;
        }
        public ConnectionMultiplexer GetNotificationHubRedis()
        {
            return _testRedis.Value;
        }
    }
}
