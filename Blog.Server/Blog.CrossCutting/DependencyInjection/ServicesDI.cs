using Blog.Domain.Interfaces.Services;
using Blog.Services.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class ServicesDI
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
        }
    }
}
