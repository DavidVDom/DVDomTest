using DavidVDom.Infraestructure;
using Microsoft.EntityFrameworkCore;

namespace DavidVDom.API.Configuration
{
    public static class DbConfiguration
    {

        public static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DavidVDomDbContext>(options => options.UseSqlServer("connectionstringquehayquesacardealgúnvault"));

            return services;
        }
    }
}
