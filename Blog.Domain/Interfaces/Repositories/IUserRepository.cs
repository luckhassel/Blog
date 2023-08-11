using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<UserEntity>
    {
        Task<UserEntity?> GetByEmailAsync(string email);
    }
}
