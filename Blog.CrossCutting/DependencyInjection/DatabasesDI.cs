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
                var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

                if (environment is not null && environment == "TEST")
                {
                    options.UseInMemoryDatabase("DbTest");
                }

                else
                {
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly("Blog"));
                }
            });
        }
    }
}
