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

namespace Blog.Test.Features.News.Integration
{
    public class NewsControllerTest : TestBaseFixture
    {
        private readonly INewsHandler _newsHandler;
        private readonly ILogger<NewsController> _logger;

        public NewsControllerTest() : base(new WebApplicationFactoryTest<Program>())
        {
            _newsHandler = Substitute.For<INewsHandler>();
            _logger = Substitute.For<ILogger<NewsController>>();
        }

        [Fact]
        public async Task CreateNews_ShouldCreateANews_WhenRequestIsValid()
        {
            var requestUserPreviously = new CreateUserRequestViewModelBuilder()
                .WithFirstName("fake")
                .WithLastName("FakeTest")
                .WithEmail("teste@teste.com")
                .WithPassword("teste123")
                .Build();

            var requestToApiEndpoint = new CreateNewsRequestViewModel()
            {
                Title = "Title Teste",
                Description = "Description Teste",
            };

            await CreateUserPreviouslyAsync(requestUserPreviously);

            //Action
            var url = "/api/News";
            var response = await _client.PostAsJsonAsync(url, requestToApiEndpoint);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                await RetrieveNewTokenAuthorization.RetrieveNewTokenToTestEndpoint(_client, ServiceProvider);

                response = await _client.PostAsJsonAsync(url, requestToApiEndpoint);
            }

            //Assertion
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();

            var content = await response.Content.ReadFromJsonAsync<CreateNewsResponseViewModel>();

            content?.Id
                .Should()
                .NotBeEmpty();
        }

        [Fact]
        public async Task GetNewsById_ShouldReturn_News()
        {
            var requestUserPreviously = new CreateUserRequestViewModelBuilder()
              .WithFirstName("Teste")
              .WithLastName("Doteste")
              .WithEmail("teste@teste.com")
              .WithPassword("teste123")
              .Build();

            var requestNewsPreviously = new CreateNewsRequestViewModel()
            {
                Title = "Title Teste",
                Description = "Description Teste",
            };

            var expectedNews = await CreateNewsPreviouslyAsync(requestUserPreviously, requestNewsPreviously);

            var guidId = expectedNews.Value.Id;

            //Action
            var url = $"/api/News/{guidId}";
            var response = await _client.GetAsync(url);

            //Assertion
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();

            var newsResponse = await response.Content.ReadFromJsonAsync<Result<GetNewsResponseViewModel>>();

            newsResponse?.Value
                .Should()
                .BeEquivalentTo(expectedNews.Value);
        }

        [Fact]
        public async Task ListNews_ShouldReturn_NewsList()
        {
            await DeleteRegistersPreviouslyInDatabase();

            var requestUserPreviously1 = new CreateUserRequestViewModelBuilder()
                .WithFirstName("FakeTest")
                .WithLastName("FakeTest")
                .WithEmail("teste@teste.com")
                .WithPassword("teste123")
                .Build();

            var requestNewsPreviously1 = new CreateNewsRequestViewModel()
            {
                Title = "Title Teste",
                Description = "Description Teste",
            };

            var requestUserPreviously2 = new CreateUserRequestViewModelBuilder()
                 .WithFirstName("Teste2")
                 .WithLastName("Doteste2")
                 .WithEmail("teste@teste2.com")
                 .WithPassword("teste1234")
                 .Build();

            var requestNewsPreviously2 = new CreateNewsRequestViewModel()
            {
                Title = "Title Teste v2",
                Description = "Description Teste v2",
            };

            await CreateNewsPreviouslyAsync(requestUserPreviously1, requestNewsPreviously1);
            await CreateNewsPreviouslyAsync(requestUserPreviously2, requestNewsPreviously2);

            //Action
            var url = "/api/News";
            var response = await _client.GetAsync(url);

            //Assertion
            response.IsSuccessStatusCode
                .Should()
                .BeTrue();

            var content = await response.Content.ReadFromJsonAsync<Result<List<GetNewsResponseViewModel>>>();

            content?.Value
                .Should()
                .HaveCount(2);
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

        private async Task<Result<GetNewsResponseViewModel>> CreateNewsPreviouslyAsync(CreateUserRequestViewModel requestUser, CreateNewsRequestViewModel request)
        {
            var repository = ServiceProvider.GetService<INewsRepository>();
            var repositoryUser = ServiceProvider.GetService<IUserRepository>();
            var service = ServiceProvider.GetService<IAuthenticationService>();

            var hashedPassword = service.HashPassword(requestUser.Password);

            var user = UserEntity.Create(requestUser.FirstName, requestUser.LastName, requestUser.Email, hashedPassword).Value;
            await repositoryUser.Create(user);
            await repositoryUser.Commit();

            var news = NewsEntity.Create(request.Title, request.Description, user.Id).Value;
            await repository.Create(news);
            await repository.Commit();

            return new GetNewsResponseViewModel
            { 
                Title = news.Title,
                Description = news.Description, 
                Id = news.Guid,
                Author = new GetUserResponseViewModel { Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, Id = user.Guid}
            };
        }

        private async Task DeleteRegistersPreviouslyInDatabase()
        {
            var repository = ServiceProvider.GetService<INewsRepository>();
            var repositoryUser = ServiceProvider.GetService<IUserRepository>();

            var users = repositoryUser.List().ToList();
            var newsList = repository.List().ToList();

            foreach (var user in users)
            {
                repositoryUser.Delete(user);
            }

            foreach (var news in newsList)
            {
                repository.Delete(news);
            }

            if (users.Any())
                await repositoryUser.Commit();

            if (newsList.Any())
                await repository.Commit();
        }
    }
}
