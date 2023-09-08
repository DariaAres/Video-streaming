using Newtonsoft.Json;
using StackExchange.Redis;

namespace VideoStreaming.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDatabase _database;

        public CacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<T?> GetObjectAsync<T>(string key)
        {
            var json = await GetStringAsync(key);

            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            var obj = JsonConvert.DeserializeObject<T>(json!);

            return obj;
        }

        public async Task<object?> GetObjectAsync(string key)
        {
            return await GetObjectAsync<object>(key);
        }

        public async Task<string?> GetStringAsync(string key)
        {
            var json = await _database.StringGetAsync(key);

            return json;
        }

        public async Task SetObjectAsync(string key, object value, TimeSpan? expiry = default)
        {
            if (value == null)
            {
                return;
            }

            var json = JsonConvert.SerializeObject(value);

            await SetStringAsync(key, json, expiry);
        }

        public async Task SetStringAsync(string key, string value, TimeSpan? expiry = default)
        {
            if (string.IsNullOrEmpty(value))
            {
                return;
            }

            await _database.StringSetAsync(key, value, expiry ?? TimeSpan.FromHours(1));
        }
    }
}
