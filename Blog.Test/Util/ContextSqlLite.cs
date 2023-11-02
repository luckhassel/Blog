using Blog.Infra.Contexts;
using Blog.Test.Util.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Blog.Test.Util
{
    public class ContextSqlLite : BlogContext
    {
        public ContextSqlLite() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(WebApplicationFactoryTest<Program>.connectionString);
        }

        public ContextSqlLite(DbContextOptions<BlogContext> options) : base(options)
        {
        }
    }
}
