using Blog.Infra.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Blog.Test.Util.Factories
{
    [Collection("Database")]
    public class WebApplicationFactoryTest<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
    {
        private readonly DbFixture _dbFixture;

        public WebApplicationFactoryTest(DbFixture dbFixture)
        {
            _dbFixture = dbFixture;
        }
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {           
            builder.UseEnvironment("Test");
            
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new[]
                {
                    new KeyValuePair<string, string>(
                        "ConnectionStrings:DefaultConnection", _dbFixture.ConnString)
                });
            });
        }
    }
}