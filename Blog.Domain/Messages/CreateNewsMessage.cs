namespace Blog.Domain.Messages
{
    public class CreateNewsMessage : Message
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AuthorEmail { get; set; } = string.Empty;
    }
}