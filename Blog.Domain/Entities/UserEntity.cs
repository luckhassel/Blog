using Blog.Domain.Models.Shared;

namespace Blog.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Password { get; private set; } = string.Empty;
        public IReadOnlyList<NewsEntity> News { get; private set; } = new List<NewsEntity>();

        private UserEntity(string firstName, string lastName, string email, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Guid = Guid.NewGuid();
        }

        public static Result<UserEntity> Create(string firstName, string lastName, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return Result.Failure<UserEntity>(Error.Create(2, "Firstname cannot be null"));

            if (string.IsNullOrWhiteSpace(lastName))
                return Result.Failure<UserEntity>(Error.Create(2, "Lastname cannot be null"));

            if (string.IsNullOrWhiteSpace(email))
                return Result.Failure<UserEntity>(Error.Create(2, "Email cannot be null"));

            if (string.IsNullOrWhiteSpace(password))
                return Result.Failure<UserEntity>(Error.Create(2, "Password cannot be null"));

            return new UserEntity(firstName, lastName, email, password);
        }
    }
}
