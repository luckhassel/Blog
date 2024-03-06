using Blog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Mappings
{
    public static class UserEntityMap
    {
        public static void ConfigureUserEntityMap(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>(i =>
            {
                i.ToTable("Users");
                i.HasKey(i => i.Id);
                i.Property(i => i.Id).ValueGeneratedOnAdd();
                i.HasMany(i => i.News).WithOne(n => n.Author).HasForeignKey(a => a.AuthorId);
            });
        }
    }
}
