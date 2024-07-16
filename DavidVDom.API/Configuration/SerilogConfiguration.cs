using Serilog;

namespace DavidVDom.API.Configuration
{
    public static class SerilogConfiguration
    {
        public static WebApplicationBuilder ConfigureSerilog(this WebApplicationBuilder builder)
        {
            builder.Host.UseSerilog((context, services, configuration) => configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .ReadFrom.Services(services));

            return builder;
        }
    }
}
