// using Azure.Core;
// using Blog.Application.Handlers;
// using Blog.Application.ViewModels;
// using Blog.Domain.Entities;
// using Blog.Domain.Interfaces.Repositories;
// using Blog.Domain.Interfaces.Services;
// using Blog.Domain.Models.Shared;
// using Blog.Test.Util;
// using Blog.Test.Util.Builders;
// using Blog.Test.Util.Factories;
// using FluentAssertions;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.DependencyInjection;
// using Moq;
// using NSubstitute;
// using System;
// using System.Net.Http;
// using System.Security.Claims;
// using Xunit;

// namespace Blog.Test.Features.News.Unit
// {
//     public class NewsHandlerTests
//     {
//         private readonly IHttpContextAccessor _httpContextAccessor;
//         private readonly INewsRepository _newsRepository;
//         private readonly IUserRepository _userRepository;
//         private readonly IAuthenticationService _authenticationService;
//         private NewsHandler _newsHandler;

//         public NewsHandlerTests()
//         {
//             _httpContextAccessor = Substitute.For<IHttpContextAccessor>();

//             var httpContext = Substitute.For<HttpContext>();
//             var user = Substitute.For<ClaimsPrincipal>();
//             var emailClaim = new Claim(ClaimTypes.Email, "teste@teste.com");
//             user.Claims.Returns(new[] { emailClaim });

//             _httpContextAccessor.HttpContext.Returns(httpContext);
//             httpContext.User.Returns(user);

//             _newsRepository = Substitute.For<INewsRepository>();
//             _userRepository = Substitute.For<IUserRepository>();
//             _authenticationService = Substitute.For<IAuthenticationService>();

//             _newsHandler = new NewsHandler(_newsRepository, _httpContextAccessor, _userRepository);
//         }

//         [Fact]
//         public async Task Create_ShouldCreateNews()
//         {
//             //Arrange
//             var request = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName("Teste")
//                 .WithLastName("1")
//                 .WithEmail("Teste@teste.com")
//                 .WithPassword("teste123")
//                 .Build();

//             var requestToApiEndpoint = new CreateNewsRequestViewModel()
//             {
//                 Title = "Title Teste",
//                 Description = "Description Teste",
//             };

//             _httpContextAccessor.HttpContext.User
//                 .FindFirst(ClaimTypes.Email)
//                 .Returns(new Claim(ClaimTypes.Email, "Teste@teste.com"));

//             var userEntityResponseUserRepositoryNull = await CreateUserPreviouslyAsync(request, 1);

//             _userRepository
//                 .GetByEmailAsync(request.Email)
//                 .Returns(userEntityResponseUserRepositoryNull);

//             //Act
//             var result = await _newsHandler.Create(requestToApiEndpoint);

//             //Assertion
//             await _newsRepository
//                 .Received(1)
//                 .Create(Arg.Any<NewsEntity>());

//             await _newsRepository
//                 .Received(1)
//                 .Commit();

//             result.IsSuccess
//                 .Should()
//                 .BeTrue();

//             result.Value.Id
//                 .Should()
//                 .NotBeEmpty();
//         }

//         [Fact]
//         public async Task Create_ShouldNotCreateNews_WhenAuthorEmailNotFound()
//         {
//             //Arrange
//             var requestToApiEndpoint = new CreateNewsRequestViewModel()
//             {
//                 Title = "Title Teste",
//                 Description = "Description Teste",
//             };

//             //Act
//             var result = await _newsHandler.Create(requestToApiEndpoint);

//             //Assertion
//             result.IsFailure
//                 .Should()
//                 .BeTrue();

//             result.Error?
//                 .Message
//                 .Should()
//                 .Be("Author email not found");
//         }

//         [Fact]
//         public async Task Create_ShouldNotCreateNews_WhenAuthorNotFound()
//         {
//             //Arrange
//             var request = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName("Teste")
//                 .WithLastName("1")
//                 .WithEmail("Teste@teste.com")
//                 .WithPassword("teste123")
//                 .Build();

//             var requestToApiEndpoint = new CreateNewsRequestViewModel()
//             {
//                 Title = "Title Teste",
//                 Description = "Description Teste",
//             };

//             _httpContextAccessor.HttpContext.User
//                 .FindFirst(ClaimTypes.Email)
//                 .Returns(new Claim(ClaimTypes.Email, "Teste@teste.com"));

//             _userRepository
//                 .GetByEmailAsync(It.IsAny<string>())
//                 .Returns(It.IsAny<UserEntity>());

//             //Act
//             var result = await _newsHandler.Create(requestToApiEndpoint);

//             //Assertion
//             result.IsFailure
//                 .Should()
//                 .BeTrue();

//             result.Error?
//                 .Message
//                 .Should()
//                 .Be("Author not found");
//         }

//         [Theory]
//         [InlineData(null, "Description Teste", 1)]
//         [InlineData("Title Teste", null, 1)]
//         [InlineData("Teste", "Description Teste", 0)]
//         public async Task Create_ShouldNotCreateNews_WhenRequestIsInvalid(string title, string description, int authorId)
//         {
//             //Arrange
//             var requestToApiEndpoint = new CreateNewsRequestViewModel()
//             {
//                 Title = title,
//                 Description = description,
//             };

//             var request = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName("Teste")
//                 .WithLastName("1")
//                 .WithEmail("Teste@teste.com")
//                 .WithPassword("teste123")
//                 .Build();

//             var userEntityResponseUserRepositoryNull = await CreateUserPreviouslyAsync(request, authorId);

//             var newsEntityCreatedPreviously = NewsEntity.Create(title, description, authorId).Value;

//             _httpContextAccessor.HttpContext.User
//                 .FindFirst(ClaimTypes.Email)
//                 .Returns(new Claim(ClaimTypes.Email, "Teste@teste.com"));

//             _userRepository
//                .GetByEmailAsync(request.Email)
//                .Returns(userEntityResponseUserRepositoryNull);

//             //Act
//             var result = await _newsHandler.Create(requestToApiEndpoint);

//             //Assertion
//             result.IsSuccess
//                 .Should()
//                 .BeFalse();

//             result.Error?
//                 .Message
//                 .Should()
//                 .ContainAny("cannot be null");
//         }

//         [Fact]
//         public async Task Get_ShouldGetNewsById_WhenRequestIsValid()
//         {
//             //Arrange
//             var requestUserPreviously = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName("Teste")
//                 .WithLastName("1")
//                 .WithEmail("Teste@teste.com")
//                 .WithPassword("teste123")
//                 .Build();

//             var requestNewsPreviously = new CreateNewsRequestViewModel()
//             {
//                 Title = "Title Teste",
//                 Description = "Description Teste",
//             };

//             var newsEntity = await CreateNewsPreviouslyAsync(requestUserPreviously, requestNewsPreviously);

//             var expectedResponseNews = new GetNewsResponseViewModel
//             {
//                 Title = newsEntity.Title,
//                 Description = newsEntity.Description,
//                 Id = newsEntity.Guid,
//                 Author = new GetUserResponseViewModel { Email = newsEntity.Author.Email, FirstName = newsEntity.Author.FirstName, LastName = newsEntity.Author.LastName, Id = newsEntity.Author.Guid }
//             };

//             var request = new GetNewsRequestViewModel() { Id = newsEntity.Guid };

//             _newsRepository
//                .Get(request.Id)
//                .Returns(newsEntity);

//             //Act
//             var result = await _newsHandler.Get(request);

//             //Assertion
//             result.IsSuccess
//                 .Should()
//                 .BeTrue();

//             result.Value
//                 .Should()
//                 .BeEquivalentTo(expectedResponseNews);
//         }

//         [Fact]
//         public async Task Get_ShouldNotGetNews_WhenNotExistsNews()
//         {
//             //Arrange
//             var request = new GetNewsRequestViewModel() { Id = new Guid() };

//             _newsRepository
//                 .Get(It.IsAny<Guid>())
//                 .Returns(It.IsAny<NewsEntity>());

//             //Act
//             var result = await _newsHandler.Get(request);

//             //Assertion
//             result.IsFailure
//                    .Should()
//                    .BeTrue();

//             result.Error?
//                 .Message
//                 .Should()
//                 .Be("news not found");
//         }

//         [Fact]
//         public async Task List_ShouldListNews_WhenRequestIsValid()
//         {
//             //Arrange
//             var requestUserPreviously1 = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName("Teste")
//                 .WithLastName("Doteste")
//                 .WithEmail("teste@teste.com")
//                 .WithPassword("teste123")
//                 .Build();

//             var requestNewsPreviously1 = new CreateNewsRequestViewModel()
//             {
//                 Title = "Title Teste",
//                 Description = "Description Teste",
//             };

//             var requestUserPreviously2 = new CreateUserRequestViewModelBuilder()
//                  .WithFirstName("Teste2")
//                  .WithLastName("Doteste2")
//                  .WithEmail("teste@teste2.com")
//                  .WithPassword("teste1234")
//                  .Build();

//             var requestNewsPreviously2 = new CreateNewsRequestViewModel()
//             {
//                 Title = "Title Teste v2",
//                 Description = "Description Teste v2",
//             };

//             var news1 = await CreateNewsPreviouslyAsync(requestUserPreviously1, requestNewsPreviously1);
//             var news2 = await CreateNewsPreviouslyAsync(requestUserPreviously2, requestNewsPreviously2);

//             var listNews = new List<NewsEntity>();
//             listNews.Add(news1);
//             listNews.Add(news2);

//             _newsRepository
//                 .List()
//                 .Returns(listNews);

//             //Act
//             var result = _newsHandler.List();

//             //Assertion
//             result.IsSuccess
//                 .Should()
//                 .BeTrue();

//             result.Value
//                 .Should()
//                 .HaveCount(2);
//         }

//         [Fact]
//         public void List_ShouldNotListNews_WhenNewsNotFound()
//         {
//             //Arrange
//             _newsRepository
//                 .List()
//                 .Returns(new List<NewsEntity>());

//             //Act
//             var result = _newsHandler.List();

//             //Assertion
//             result.IsFailure
//                 .Should()
//                 .BeTrue();

//             result.Error?.Message
//                 .Should()
//                 .Be("news not found");
//         }

//         private async Task<UserEntity> CreateUserPreviouslyAsync(CreateUserRequestViewModel request, int authorId)
//         {
//             var repository = ServiceProvider.GetService<IUserRepository>();
//             var service = ServiceProvider.GetService<IAuthenticationService>();

//             var hashedPassword = service.HashPassword(request.Password);
//             var user = UserEntity.Create(request.FirstName, request.LastName, request.Email, hashedPassword).Value;

//             if (authorId > 0)
//             {
//                 await repository.Create(user);
//                 await repository.Commit();
//             }

//             return user;
//         }

//         private async Task<NewsEntity> CreateNewsPreviouslyAsync(CreateUserRequestViewModel requestUser, CreateNewsRequestViewModel requestNews)
//         {
//             var repository = ServiceProvider.GetService<INewsRepository>();
//             var repositoryUser = ServiceProvider.GetService<IUserRepository>();
//             var service = ServiceProvider.GetService<IAuthenticationService>();

//             var hashedPassword = service.HashPassword(requestUser.Password);

//             var user = UserEntity.Create(requestUser.FirstName, requestUser.LastName, requestUser.Email, hashedPassword).Value;
//             await repositoryUser.Create(user);
//             await repositoryUser.Commit();

//             var news = NewsEntity.Create(requestNews.Title, requestNews.Description, user.Id).Value;
//             await repository.Create(news);
//             await repository.Commit();

//             return news;
//         }
//     }
// }
