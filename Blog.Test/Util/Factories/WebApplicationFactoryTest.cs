using Blog.Infra.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Blog.Test.Util.Factories
{
    public class WebApplicationFactoryTest<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        public readonly static string ConnectionString = "Data Source=TestDb.db";

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                RemoveAllDbContextsFromServices(services);

                services.AddDbContext<BlogContext>(options =>
                {
                    var projectAssemblyName = Assembly.GetAssembly(typeof(WebApplicationFactoryTest<>)).GetName().Name;
                    options.UseSqlite(ConnectionString, x => x.MigrationsAssembly(projectAssemblyName));
                });

                services.AddDbContext<ContextSqlLite>();

                MigrateDbContext<ContextSqlLite>(services);
            });
        }

        private void RemoveAllDbContextsFromServices(IServiceCollection services)
        {
            var descriptors = services.Where(d => d.ServiceType.BaseType == typeof(DbContextOptions)).ToList();
            descriptors.ForEach(d => services.Remove(d));

            var dbContextDescriptors = services.Where(d => d.ServiceType.BaseType == typeof(DbContext)).ToList();
            dbContextDescriptors.ForEach(d => services.Remove(d));
        }

        public void MigrateDbContext<TContext>(IServiceCollection serviceCollection) where TContext : DbContext
        {
            var serviceProvider = serviceCollection.BuildServiceProvider();

            using var scope = serviceProvider.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation("Migrating database associated with context {DbContextName}", typeof(TContext).Name);

                if (context.Database.IsSqlServer())
                {
                    throw new Exception("Use Sqlite instead of sql server!");
                }

                context.Database.EnsureDeleted();

                context.Database.Migrate();

                logger.LogInformation("Migrated database associated with context {DbContextName}", typeof(TContext).Name);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating the database used on context {DbContextName}", typeof(TContext).Name);
                throw;
            }
        }
    }
}