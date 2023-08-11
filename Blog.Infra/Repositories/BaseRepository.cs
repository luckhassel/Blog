using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        private readonly BlogContext _context;
        protected DbSet<TEntity> DbSet => _context is not null ? _context.Set<TEntity>() : throw new Exception("Null Db context");

        public BaseRepository(BlogContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }

        public virtual async Task Create(TEntity entity)
        {
            await DbSet.AddAsync(entity);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public virtual async Task<TEntity?> Get(Guid id)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.Guid == id);
        }

        public virtual IReadOnlyList<TEntity>? List()
        {
            return DbSet.ToList();

        }

        public virtual void Update(TEntity entity)
        {
            DbSet.Update(entity);
        }
    }
}
