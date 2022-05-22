
namespace RedisFundmentalUsageProject.Services
{
    public interface IRedisService
    {
        Task<T> Get<T>(string key);
        Task<HashSet<T>> GetAll<T>(string key);
        Task Add(string key, object data);
        Task Upsert<T>(string key, object data);
        Task Remove(string key);
        Task<bool> Any(string key);
    }
}
