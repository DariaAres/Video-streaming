namespace VideoStreaming.Helpers
{
    public static class RandomString
    {
        public static string Generate(int length = 10)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[Random.Shared.Next(s.Length)]).ToArray());
        }
    }
}
