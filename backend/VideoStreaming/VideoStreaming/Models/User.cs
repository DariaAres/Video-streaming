using Microsoft.AspNetCore.Identity;

namespace VideoStreaming.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = default!;
        public string Surname { get; set; } = default!;

        public int EmailConfirmationCode { get; set; }

        public string FullName { get => $"{Name} {Surname}"; }
    }
}
