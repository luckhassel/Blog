using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class TelemetryDI
    {
        public static void AddTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry();
        }
    }
}