using StackExchange.Redis;

namespace RedisFundmentalUsageProject.Services
{
    public interface IRedisManager
    {
        IDatabase TestRedisDatabase { get; }
        IServer TestRedisServer { get; }
    }
}
