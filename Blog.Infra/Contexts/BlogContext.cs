using Blog.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Contexts
{
    public class BlogContext : DbContext
    {
        protected BlogContext()
        {
        }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) 
        { 
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(Console.WriteLine)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureNewsEntityMap();
            modelBuilder.ConfigureUserEntityMap();
        }
    }
}
