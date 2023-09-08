namespace VideoStreaming.Services
{
    public interface ICacheService
    {
        Task SetStringAsync(string key, string value, TimeSpan? expiry = default);

        Task<string?> GetStringAsync(string key);

        Task SetObjectAsync(string key, object value, TimeSpan? expiry = default);

        Task<T?> GetObjectAsync<T>(string key);

        Task<bool> DeleteAsync(string key);
    }
}
