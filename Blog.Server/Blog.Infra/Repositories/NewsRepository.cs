using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Repositories
{
    public class NewsRepository : BaseRepository<NewsEntity>, INewsRepository
    {
        public NewsRepository(BlogContext context) : base(context)
        {
        }

        public override async Task<NewsEntity?> Get(Guid id)
        {
            var query = DbSet.Include(n => n.Author);

            return await query.FirstOrDefaultAsync(n => n.Guid == id);
        }

        public override IReadOnlyList<NewsEntity>? List()
        {
           var query = DbSet.Include(n => n.Author);

            return query.ToList();
        }
    }
}
