using VideoStreaming.Models;

namespace VideoStreaming.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateTokenAsync(User user);
    }
}
