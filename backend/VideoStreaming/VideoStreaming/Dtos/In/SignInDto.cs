namespace VideoStreaming.Dtos.In
{
    public class SignInDto
    {
        /// <summary>
        /// might be an email
        /// </summary>
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
