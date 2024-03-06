using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Mappings
{
    public static class NewsEntityMap
    {
        public static void ConfigureNewsEntityMap(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsEntity>(i =>
            {
                i.ToTable("News");
                i.HasKey(i => i.Id);
                i.Property(i => i.Id).ValueGeneratedOnAdd();
                i.HasOne(i => i.Author).WithMany(a => a.News);
            });
        }
    }
}
