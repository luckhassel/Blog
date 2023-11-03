using Blog.Domain.Interfaces.Repositories;
using Blog.Infra.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Test.Util.Factories
{
    public class WebApplicationFactoryTest<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        public readonly static string connectionString = "DataSource=:memory:";
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "TEST");

            //builder.ConfigureServices(services =>
            //{
            //    builder.ConfigureTestServices(async services =>
            //    {
            //        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BlogContext>));
            //        if (descriptor != null) services.Remove(descriptor);

            //        services
            //            .AddEntityFrameworkSqlite()
            //            .AddDbContext<ContextSqlLite>(options => options.UseInMemoryDatabase(connectionString));
            //    });
            //});
        }
    }
}