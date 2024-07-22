using DavidVDom.Domain.Abstractions;
using DavidVDom.Infraestructure.Repositories;
using DavidVDom.Infraestructure;
using DavidVDom.Domain.Abstractions.Repositories;

namespace DavidVDom.API.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection CustomServices(this IServiceCollection services)
        {
            // Mediator
            //services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetBankHandler).Assembly));

            // Respositories
            services.AddTransient<IBankRepository, BanksRepository>();

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
