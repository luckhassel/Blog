using Blog.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class MassTransitDI
    {
        public static void AddMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettings = new ApplicationSettings();
            configuration.GetSection(nameof(ApplicationSettings)).Bind(applicationSettings);

            services.AddMassTransit(config => 
            {
                config.UsingRabbitMq((context, cfg) => 
                {
                    cfg.Host(applicationSettings.MassTransitSettings.Server, "/", host => 
                    {
                       host.Username(applicationSettings.MassTransitSettings.User);
                       host.Password(applicationSettings.MassTransitSettings.Password); 
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}