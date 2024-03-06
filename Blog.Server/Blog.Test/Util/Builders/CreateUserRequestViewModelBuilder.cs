using Blog.Application.ViewModels;

namespace Blog.Test.Util.Builders
{
    public class CreateUserRequestViewModelBuilder
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public CreateUserRequestViewModel Build() 
        {
            return new CreateUserRequestViewModel
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Password = Password
            };
        }

        public CreateUserRequestViewModelBuilder WithFirstName(string firstName)
        {
            FirstName = firstName;
            return this;
        }
        public CreateUserRequestViewModelBuilder WithLastName(string lastName)
        {
            LastName = lastName;
            return this;
        }
        public CreateUserRequestViewModelBuilder WithEmail(string email)
        {
            Email = email;
            return this;
        }
        public CreateUserRequestViewModelBuilder WithPassword(string password)
        {
            Password = password;
            return this;
        }
    }
}
