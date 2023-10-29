using System.Text.Json.Serialization;

namespace Blog.Application.ViewModels
{
    public class CreateUserRequestViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class CreateUserResponseViewModel
    {
        public Guid Id { get; set; }
    }

    public class GetUserRequestViewModel
    {
        public Guid Id { get; set; }
    }

    public class GetUserResponseViewModel
    {
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; } 
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
    }

    public class LoginUserRequestViewModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginUserResponseViewModel
    {
        public string Token { get; set; } = string.Empty;
    }

}
