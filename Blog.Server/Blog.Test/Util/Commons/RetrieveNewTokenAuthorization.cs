using Blog.Application.ViewModels;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;
using Blog.Domain.Models.Shared;
using Blog.Test.Util.Builders;
using Blog.Test.Util.Factories;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Blog.Test.Util.Commons
{
    public static class RetrieveNewTokenAuthorization
    {
        public static async Task RetrieveNewTokenToTestEndpoint(HttpClient _client, IServiceProvider serviceProvider)
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

            await CreateUserPreviouslyAsync(requestToInsertDBUser, serviceProvider);

            var url = "/api/Users/login";
            var response = await _client.PostAsJsonAsync(url, requestToApiEndpoint);

            var loginResponse = await response.Content.ReadFromJsonAsync<Result<LoginUserResponseViewModel>>();

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",loginResponse?.Value.Token);
        }

        private static async Task<Result<GetUserResponseViewModel>> CreateUserPreviouslyAsync(CreateUserRequestViewModel request, IServiceProvider serviceProvider)
        {
            var repository = serviceProvider.GetService<IUserRepository>();
            var service = serviceProvider.GetService<IAuthenticationService>();

            var hashedPassword = service?.HashPassword(request.Password);
            var user = UserEntity.Create(request.FirstName, request.LastName, request.Email, hashedPassword).Value;

            await repository.Create(user);
            await repository.Commit();

            var responseUser = new GetUserResponseViewModel { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Id = user.Guid };

            return responseUser;
        }
    }
}
