using VideoStreaming.Helpers;

namespace VideoStreaming.Services
{
    public class ConfirmationCodeService : IConfirmationCodeService
    {
        public string GenerateCode()
        {
            return RandomString.Generate(4) + "-" + RandomString.Generate(4);
        }
    }
}
