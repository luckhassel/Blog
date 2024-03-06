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

        private NewsEntity(string title, string description, UserEntity author)
        {
            Title = title;
            Description = description;
            AuthorId = author.Id;
            PublishDate = DateTime.UtcNow;
            Guid = Guid.NewGuid();
            Author = author;
        }

        public static Result<NewsEntity> Create(string title, string description, UserEntity author)
        {
            if (string.IsNullOrWhiteSpace(title))
                return Result.Failure<NewsEntity>(Error.Create(1, "Title cannot be null"));

            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<NewsEntity>(Error.Create(1, "Description cannot be null"));

            return new NewsEntity(title, description, author);
        }
    }
}
