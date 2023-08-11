using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Infra.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Blog.Infra.Repositories
{
    public class UserRepository : BaseRepository<UserEntity>, IUserRepository
    {
        public UserRepository(BlogContext context) : base(context)
        {
             
        }

        public Task<UserEntity?> GetByEmailAsync(string email)
        {
            return DbSet.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
