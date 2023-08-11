using Blog.Application.Handlers;
using Blog.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class HandlersDI
    {
        public static void AddHandlers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUserHandler, UserHandler>();
            services.AddScoped<INewsHandler, NewsHandler>();
        }
    }
}
