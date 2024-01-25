using Blog.Domain.Interfaces.Services;
using Blog.Services.Authentication;
using Blog.Services.Email;
using Blog.Services.MessageBroker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class ServicesDI
    {
        public static void AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddScoped<IMessageBrokerService, MessageBrokerService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
