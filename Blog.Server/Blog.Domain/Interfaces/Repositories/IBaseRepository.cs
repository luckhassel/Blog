using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories
{
    public interface IBaseRepository<TEntity>
        where TEntity : BaseEntity
    {
        Task Create(TEntity entity);
        Task<TEntity?> Get(Guid id);
        IReadOnlyList<TEntity>? List();
        void Update (TEntity entity);
        void Delete (TEntity entity);
        Task Commit();
    }
}
