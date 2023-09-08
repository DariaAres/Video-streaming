using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using VideoStreaming.Constants;
using VideoStreaming.Models;
using VideoStreaming.Persistence;

namespace VideoStreaming.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static async Task<IServiceProvider> MigrateAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var provider = scope.ServiceProvider;
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(ServiceProviderExtensions));

            logger.LogInformation("Starting migrating the database");

            try
            {
                var context = provider.GetRequiredService<VideoStreamingDbContext>();

                await context.Database.MigrateAsync();

                logger.LogInformation("Database migrated sucessfuly");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error ocurred while migrating");
            }

            return serviceProvider;
        }

        public static async Task<IServiceProvider> SeedAsync(this IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            var loggerFactory = scopedProvider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger(typeof(ServiceProviderExtensions));

            try
            {
                var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scopedProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.Roles.AnyAsync())
                {
                    logger.LogInformation("Starting roles seed");

                    var roles = AuthenticationConstants.Roles.AllRoles.Select(r => new IdentityRole
                    {
                        Name = r
                    });

                    foreach (var role in roles)
                    {
                        await roleManager.CreateAsync(role);
                    }

                    logger.LogInformation("Roles seeded successfully");
                }

                var adminRole = AuthenticationConstants.Roles.Admin;
                if (!await userManager.Users.AnyAsync(u => u.UserName.ToLower() == adminRole.ToLower()))
                {
                    logger.LogInformation("Starting admin user seed");

                    var admin = new User
                    {
                        UserName = "admin",
                        EmailConfirmed = true,
                        Name = adminRole,
                        Surname = adminRole,
                    };

                    await userManager.CreateAsync(admin, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(admin, adminRole);

                    logger.LogInformation("Admin user with role seeded successfully");
                }

                logger.LogInformation("Data seeding was successful");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured while seeding data");
            }

            return provider;
        }
    }
}
