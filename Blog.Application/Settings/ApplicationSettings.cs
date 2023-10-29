namespace Blog.Application.Settings
{
    public class ApplicationSettings
    {
        public SecuritySettings SecuritySettings { get; set; }
    }

    public class SecuritySettings
    {
        public string Secret { get; set; }
    }
}
