using Microsoft.Extensions.Configuration;
using VideoStreaming.Extensions;

namespace VideoStreaming
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var configuration = builder.Configuration;

            builder.Services.AddApplicationServices(configuration);
            builder.Services.AddApplicationIdentityServices(configuration);

            builder.Services.AddControllers(options =>
            {
                options.AddDefaultFilters();
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            await app.Services.MigrateAsync();
            await app.Services.SeedAsync();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}