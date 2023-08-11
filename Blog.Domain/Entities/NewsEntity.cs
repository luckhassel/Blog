using Blog.Domain.Models.Shared;

namespace Blog.Domain.Entities
{
    public class NewsEntity : BaseEntity
    {
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public DateTime PublishDate { get; private set; }
        public int AuthorId { get; private set; }
        public UserEntity Author { get; private set; }

        private NewsEntity() { }

        private NewsEntity(string title, string description, int authorId)
        {
            Title = title;
            Description = description;
            AuthorId = authorId;
            PublishDate = DateTime.UtcNow;
            Guid = Guid.NewGuid();
        }

        public static Result<NewsEntity> Create(string title, string description, int authorId)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<NewsEntity>(Error.Create(1, "Title cannot be null"));

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<NewsEntity>(Error.Create(1, "Description cannot be null"));

            if (authorId <= 0)
                return Result.Failure<NewsEntity>(Error.Create(1, "Author cannot be null"));

            return new NewsEntity(title, description, authorId);
        }
    }
}
