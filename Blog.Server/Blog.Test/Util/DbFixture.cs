using Blog.Infra.Contexts;
using Blog.Test.Util.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace Blog.Test.Util
{
    public class DbFixture : IDisposable
    {
        private readonly BlogContext _dbContext;
        private readonly string _blogDbName = $"TestDb";
        public readonly string ConnString;
        
        private bool _disposed;

        public DbFixture()
        {
            ConnString = $"Server=localhost,1433;Database={_blogDbName};User=sa;Password=Pos2023@Fiap;TrustServerCertificate=true";

            var builder = new DbContextOptionsBuilder<BlogContext>();

            builder.UseSqlServer(ConnString);
            _dbContext = new BlogContext(builder.Options);

            _dbContext.Database.EnsureCreated();
            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // remove the temp db from the server once all tests are done
                    _dbContext.Database.EnsureDeleted();
                }

                _disposed = true;
            }
        }
    }

    [CollectionDefinition("Database")]
    public class DatabaseCollection : ICollectionFixture<DbFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
