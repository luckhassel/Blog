using Blog.Application.Interfaces;
using Blog.Application.ViewModels;
using Blog.Controllers;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;
using Blog.Domain.Models.Shared;
using Blog.Test.Util;
using Blog.Test.Util.Builders;
using Blog.Test.Util.Commons;
using Blog.Test.Util.Factories;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using System.Net.Http.Json;
using Xunit;

namespace Blog.Test.Features.Users.Integration
{
    public class UsersControllerTest : TestBaseFixture
    {
        private readonly IUserHandler _userHandler;
        private readonly ILogger<UsersController> _logger;

        public UsersControllerTest() : base(new WebApplicationFactoryTest<Program>())
        {
            _userHandler = Substitute.For<IUserHandler>();
            _logger = Substitute.For<ILogger<UsersController>>();
        }

        [Fact]
        public async Task LoginUser_ShouldLoginUser_ReturningToken()
        {
            var requestToInsertDBUser = new CreateUserRequestViewModelBuilder()
                .WithFirstName("Teste")
                .WithLastName("teste")
                .WithEmail("teste@teste.com")
                .WithPassword("teste123")
                .Build();

            var requestToApiEndpoint = new LoginUserRequestViewModel()
            {
                Email = "teste@teste.com",
                Password = "teste123"
            };

            await CreateUserPreviouslyAsync(requestToInsertDBUser);

            //Action
            var url = "/api/Users/login";
            var response = await _client.PostAsJsonAsync(url, requestToApiEndpoint);

            //Assertion
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();

            var loginResponse = await response.Content.ReadFromJsonAsync<Result<LoginUserResponseViewModel>>();

            loginResponse?.Value
                .Should()
                .NotBeNull();
        }

        [Fact]
        public async Task GetUserById_ShouldReturn_User()
        {
            var request = new CreateUserRequestViewModelBuilder()
              .WithFirstName("Teste")
              .WithLastName("Doteste")
              .WithEmail("Teste@teste.com")
              .WithPassword("teste123")
              .Build();

            var expectedUser = await CreateUserPreviouslyAsync(request);

            var guidId = expectedUser.Value.Id;

            //Action
            var url = $"/api/Users/{guidId}";
            var response = await _client.GetAsync(url);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RetrieveNewTokenAuthorization.RetrieveNewTokenToTestEndpoint(_client, ServiceProvider);

                response = await _client.GetAsync(url);
            }

            //Assertion
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();

            var userResponse = await response.Content.ReadFromJsonAsync<Result<GetUserResponseViewModel>>();

            userResponse?.Value
                .Should()
                .BeEquivalentTo(expectedUser.Value);
        }

        [Fact]
        public async Task CreateUser_ShouldReturn_UserCreatedId()
        {
            //Arrange
            var request = new CreateUserRequestViewModelBuilder()
                .WithFirstName("Teste")
                .WithLastName("Do teste")
                .WithEmail("Teste@teste.com")
                .WithPassword("teste123")
                .Build();

            //Action
            var url = "/api/Users";
            var response = await _client.PostAsJsonAsync(url, request);

            //Assertion
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();

            var content = await response.Content.ReadFromJsonAsync<CreateUserResponseViewModel>();

            content?.Id
                .Should()
                .NotBeEmpty();
        }

        private async Task<Result<GetUserResponseViewModel>> CreateUserPreviouslyAsync(CreateUserRequestViewModel request)
        {
            var repository = ServiceProvider.GetService<IUserRepository>();
            var service = ServiceProvider.GetService<IAuthenticationService>();

            var hashedPassword = service.HashPassword(request.Password);
            var user = UserEntity.Create(request.FirstName, request.LastName, request.Email, hashedPassword).Value;

            await repository.Create(user);
            await repository.Commit();

            var responseUser = new GetUserResponseViewModel { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Id = user.Guid };

            return responseUser;
        }
    }
}
