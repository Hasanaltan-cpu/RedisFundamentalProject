using Newtonsoft.Json;

namespace RedisFundmentalUsageProject.Services
{
    public class RedisService : IRedisService
    {
        private readonly IRedisManager _redisManager;

        public RedisService(IRedisManager redisManager)
        {
            _redisManager = redisManager ?? throw new ArgumentNullException(nameof(redisManager));
        }

        public async Task<HashSet<T>> GetAll<T>(string key)
        {
            var allKeys = await Task.Run(() => _redisManager.TestRedisServer.Keys(database: 0, pattern: key, pageSize: int.MaxValue).ToArray());

            var values = await _redisManager.TestRedisDatabase.StringGetAsync(allKeys);
            var devices = new HashSet<T>();
            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind //Gelen veri utc ise utc'ye local date  ise localdate'e çevirir
            };
            foreach (var item in values)
            {
                if (!item.IsNullOrEmpty)
                {
                    T deserializeObject = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(item.ToString(), settings);
                    devices.Add(deserializeObject);
                }
            }

            return devices;
        }
        public async Task Add(string key, object data)
        {

            await _redisManager.TestRedisDatabase.StringSetAsync(key, JsonConvert.SerializeObject(data));
        }

        public async Task Upsert<T>(string key, object data)
        {
            if (await _redisManager.TestRedisDatabase.KeyExistsAsync(key))
            {
                var getNewDataIsNotNullProps = data.GetType().GetProperties()
                            .Select(x => new { property = x.Name, value = x.GetValue(data) })
                            .Where(x => x.value != null)
                            .ToList();

                var jsonData = await _redisManager.TestRedisDatabase.StringGetAsync(key);
                var existKeyModel = JsonConvert.DeserializeObject<T>(jsonData);
                if (existKeyModel != null)
                {
                    System.Reflection.PropertyInfo[] existKeyModelProps = existKeyModel.GetType().GetProperties();
                    foreach (System.Reflection.PropertyInfo existProps in existKeyModelProps)
                    {
                        getNewDataIsNotNullProps.ForEach(n =>
                        {
                            if (existProps.Name == n.property.ToString())
                            {
                                if (n.value.GetType() == typeof(int))
                                {
                                    var _value = (int)n.value;
                                    if (_value > 0)
                                        existProps.SetValue(existKeyModel, n.value);
                                }
                                else
                                {
                                    existProps.SetValue(existKeyModel, n.value);
                                }
                            }
                        });
                    }
                    await _redisManager.TestRedisDatabase.StringSetAsync(key, JsonConvert.SerializeObject(existKeyModel));
                }
                else
                {
                    await _redisManager.TestRedisDatabase.StringSetAsync(key, JsonConvert.SerializeObject(data));
                }
            }
            else
            {
                await _redisManager.TestRedisDatabase.StringSetAsync(key, JsonConvert.SerializeObject(data));
            }
        }

        public async Task<bool> Any(string key)
        {
            return await _redisManager.TestRedisDatabase.KeyExistsAsync(key);
        }


        public async Task<T> Get<T>(string key)
        {
            if (await Any(key))
            {
                string jsonData = await _redisManager.TestRedisDatabase.StringGetAsync(key);
                return JsonConvert.DeserializeObject<T>(jsonData);
            }
            else return default(T);
        }

        public async Task Remove(string key)
        {
            await _redisManager.TestRedisDatabase.KeyDeleteAsync(key);
        }
    }
}
