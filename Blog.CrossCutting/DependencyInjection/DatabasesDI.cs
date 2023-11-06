using Blog.Infra.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.CrossCutting.DependencyInjection
{
    public static class DatabasesDI
    {
        public static void AddDatabases(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BlogContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("Blog"));
            });
        }
    }
}
