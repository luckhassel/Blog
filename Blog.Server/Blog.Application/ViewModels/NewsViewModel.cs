namespace Blog.Application.ViewModels
{
    public class CreateNewsRequestViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateNewsResponseViewModel
    {
        public Guid Id { get; set; }
    }

    public class GetNewsRequestViewModel
    {
        public Guid Id { get; set; }
    }

    public class GetNewsResponseViewModel
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public GetUserResponseViewModel Author { get; set; } = new();
    }

}
