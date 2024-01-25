using Blog.Application.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class ApplicationSettingsDI
    {
        public static void AddSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var applicationSettings = new ApplicationSettings();
            configuration.GetSection(nameof(ApplicationSettings)).Bind(applicationSettings);

            services.AddSingleton(applicationSettings);
        }
    }
}
