using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DavidVDom.Domain.Abstractions;
using DavidVDom.Infraestructure;
using DavidVDom.Infraestructure.Seeds;
using Microsoft.EntityFrameworkCore;

namespace DavidVDom.API.Configuration
{
    public static class DbConfiguration
    {
        public static WebApplication DatabaseInitialization(this WebApplication app)
        {
            DatabaseExists(app);
            ApplyMigrations(app);
            SeedDatabase(app);

            return app;
        }

        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, WebApplicationBuilder builder)
        {
            var connectionString = GetConnectionString(builder);
            services.AddDbContext<DavidVDomDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        private static string GetConnectionString(WebApplicationBuilder builder)
        {
            string connectionString = string.Empty;

            // in this example only for development
            if (builder.Environment.IsDevelopment())
            {
                var keyVaultUri = builder.Configuration.GetSection("KeyVault:KeyVaultURL").Value;
                var clientId = builder.Configuration.GetSection("KeyVault:ClientId").Value;
                var clientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret").Value;
                var directoryId = builder.Configuration.GetSection("KeyVault:DirectoryId").Value;

                var credential = new ClientSecretCredential(directoryId, clientId, clientSecret);
                var secretClient = new SecretClient(new Uri(keyVaultUri), credential);

                connectionString = secretClient.GetSecret("davidvdom-connectionstring").Value.Value;
            }

            return connectionString;
        }

        public static void DatabaseExists(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DavidVDomDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            if (!context.Database.CanConnect())
            {
                string errorMessage = "Database doesn't exists.";
                logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }

        public static void ApplyMigrations(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DavidVDomDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            if (context.Database.HasPendingModelChanges())
            {
                string errorMessage = "Database model has pending migrations";
                logger.LogError(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }

        public async static void SeedDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DavidVDomDbContext>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            if (context.Banks.OrderBy(x => x.Id).FirstOrDefault() == null)
            {
                logger.LogInformation("Empty bank table");
                try
                {
                    logger.LogInformation("Seeding database...");
                    await InitialSeed.Seed(context, unitOfWork, logger);
                    logger.LogInformation("Seeding completed");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex.InnerException, "Error seeding database: ");
                    throw;
                }
            }
        }
    }
}
