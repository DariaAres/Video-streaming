using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;
using VideoStreaming.Common;
using VideoStreaming.Constants;
using VideoStreaming.Models;
using VideoStreaming.Options;
using VideoStreaming.Persistence;
using VideoStreaming.Services;

namespace VideoStreaming.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: "MyPolicy",
                    policy =>
                    {
                        policy.AllowAnyOrigin();
                        policy.AllowAnyHeader();
                        policy.AllowAnyMethod();
                    });
            });

            services.AddDbContext<VideoStreamingDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("Default")));

            services.AddSingleton<IConnectionMultiplexer>((provider) =>
            {
                try
                {
                    return ConnectionMultiplexer.Connect(
                        configuration.GetConnectionString("Redis"));
                }
                catch
                {
                    return null!;
                }
            });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IEmailSender, GmailSender>();
            services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IConfirmationCodeService, ConfirmationCodeService>();
            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IFileService, FileService>();

            //services.AddHostedService<ContractsCreatorWorker>();
            //services.AddHostedService<FakeOrdersCreatorWorker>();

            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Program>(ServiceLifetime.Scoped);

            services.AddAutoMapper(typeof(AutomapperProfile).Assembly);

            services.Configure<GmailSettings>(configuration.GetSection(GmailSettings.configurationSection));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Rovie API",
                    Description = "Rovie API Swagger",
                    Contact = new OpenApiContact
                    {
                        Name = "Rovie",
                        Email = "both.watch.app@gmail.com",
                    }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddAzureClients(clientBuilder =>
            {
                var storageUrl = configuration.GetConnectionString("Blob");

                clientBuilder.AddBlobServiceClient(storageUrl);
            });

            return services;
        }

        public static IServiceCollection AddApplicationIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddUserManager<UserManager<User>>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddSignInManager<SignInManager<User>>()
                .AddRoleValidator<RoleValidator<IdentityRole>>()
                .AddEntityFrameworkStores<VideoStreamingDbContext>();

            var key = configuration[ConfigurationConstants.TokenSecurityKey];
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            return services;
        }
    }
}
