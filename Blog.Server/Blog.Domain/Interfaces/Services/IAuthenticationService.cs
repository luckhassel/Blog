using Blog.Domain.Entities;

namespace Blog.Domain.Interfaces.Services
{
    public interface IAuthenticationService
    {
        string HashPassword(string password);
        bool IsPasswordCorrect(string password, string passwordHashed);
        string GenerateToken(UserEntity user);
    }
}
