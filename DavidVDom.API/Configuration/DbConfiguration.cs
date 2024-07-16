using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DavidVDom.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace DavidVDom.API.Configuration
{
    public static class DbConfiguration
    {

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
    }
}
