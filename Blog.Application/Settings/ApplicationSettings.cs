namespace Blog.Application.Settings
{
    public class ApplicationSettings
    {
        public SecuritySettings SecuritySettings { get; set; } = new();
    }

    public class SecuritySettings
    {
        public string Secret { get; set; } = string.Empty;
    }
}
