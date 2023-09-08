namespace VideoStreaming.Constants
{
    public static class AuthenticationConstants
    {
        public static class Roles
        {
            public const string Admin = "Admin";
            public const string User = "User";

            public static readonly string[] AllRoles = new string[]
            {
                Admin,
                User,
            };
        }

        public static class CustomClaimTypes
        {
            public const string Roles = "roles";
        }
    }
}
