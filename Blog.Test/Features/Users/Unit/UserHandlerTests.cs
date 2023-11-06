// using Blog.Application.Handlers;
// using Blog.Application.ViewModels;
// using Blog.Domain.Entities;
// using Blog.Domain.Interfaces.Repositories;
// using Blog.Domain.Interfaces.Services;
// using Blog.Test.Util.Builders;
// using FluentAssertions;
// using Moq;
// using NSubstitute;
// using Xunit;

// namespace Blog.Test.Features.Users.Unit
// {
//     public class NewsHandlerTests
//     {
//         private readonly IAuthenticationService _authenticationService;
//         private readonly IUserRepository _userRepository;
//         private UserHandler _userHandler;

//         public NewsHandlerTests()
//         {
//             _authenticationService = Substitute.For<IAuthenticationService>();
//             _userRepository = Substitute.For<IUserRepository>();

//             _userHandler = new UserHandler(_userRepository, _authenticationService);
//         }

//         [Fact]
//         public async Task CreateAsync_ShouldCreateUser_WhenRequestIsValid()
//         {
//             //Arrange
//             var request = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName("Teste")
//                 .WithLastName("1")
//                 .WithEmail("Teste@teste.com")
//                 .WithPassword("teste123")
//                 .Build();

//             _userRepository
//                 .GetByEmailAsync(It.IsAny<string>())
//                 .Returns(It.IsAny<UserEntity>());

//             _authenticationService
//                 .HashPassword(request.Password)
//                 .Returns("slgfhjsdajg1!!#!)($%@1!@@");

//             //Act
//             var result = await _userHandler.CreateAsync(request);

//             //Assertion
//             await _userRepository
//                 .Received(1)
//                 .Create(Arg.Any<UserEntity>());

//             await _userRepository
//                 .Received(1)
//                 .Commit();

//             result.IsSuccess
//                 .Should()
//                 .BeTrue();

//             result.Value
//                 .Should()
//                 .BeOfType<CreateUserResponseViewModel>();
//         }

//         [Fact]
//         public async Task CreateAsync_ShouldNotCreateUser_WhenEmailIsAlreadyInUse()
//         {
//             //Arrange
//             var request = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName("Teste")
//                 .WithLastName("1")
//                 .WithEmail("Teste@teste.com")
//                 .WithPassword("teste123")
//                 .Build();

//             var userEntityResponseUserRepositoryNull = UserEntity.Create(request.FirstName, request.LastName, request.Email, request.Password).Value;

//             _userRepository
//                 .GetByEmailAsync(request.Email)
//                 .Returns(userEntityResponseUserRepositoryNull);

//             //Act
//             var result = await _userHandler.CreateAsync(request);

//             //Assertion
//             result.IsFailure
//                 .Should()
//                 .BeTrue();

//             result.Error?
//                 .Message
//                 .Should()
//                 .Be("Email already in use");
//         }

//         [Theory]
//         [InlineData(null, "1", "teste@teste.com", "teste123")]
//         [InlineData("Teste", null, "teste@teste.com", "teste123")]
//         [InlineData("Teste", "1", null, "teste123")]
//         [InlineData("Teste", "1", "teste@teste.com", null)]
//         public async Task CreateAsync_ShouldNotCreateUser_WhenRequestIsInvalid(string firstName, string lastName, string email, string password)
//         {
//             //Arrange
//             var request = new CreateUserRequestViewModelBuilder()
//                 .WithFirstName(firstName)
//                 .WithLastName(lastName)
//                 .WithEmail(email)
//                 .WithPassword(password)
//                 .Build();

//             var userEntityResponseUserRepositoryNull = UserEntity.Create(request.FirstName, request.LastName, request.Email, request.Password).Value;

//             _userRepository
//                .GetByEmailAsync(request.Email)
//                .Returns(userEntityResponseUserRepositoryNull);

//             if(request.Password != null)
//             {
//                 _authenticationService
//                   .HashPassword(request.Password)
//                   .Returns("slgfhjsdajg1!!#!)($%@1!@@");
//             }

//             //Act
//             var result = await _userHandler.CreateAsync(request);

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
//         public async Task GetAsync_ShouldGetUser_WhenRequestIsValid()
//         {
//             //Arrange
//             var userEntityResponseUserRepository = UserEntity.Create("Teste", "1", "teste@teste.com", "teste123").Value;

//             var request = new GetUserRequestViewModel() { Id = userEntityResponseUserRepository.Guid };

//             var response = new GetUserResponseViewModel
//             {
//                 Email = userEntityResponseUserRepository.Email,
//                 FirstName = userEntityResponseUserRepository.FirstName,
//                 LastName = userEntityResponseUserRepository.LastName,
//                 Id = userEntityResponseUserRepository.Guid
//             };

//             _userRepository
//                .Get(request.Id)
//                .Returns(userEntityResponseUserRepository);

//             //Act
//             var result = await _userHandler.GetAsync(request);

//             //Assertion
//             await _userRepository
//                 .Received(1)
//                 .Get(request.Id);

//             result.IsSuccess
//                 .Should()
//                 .BeTrue();

//             result.Value
//                 .Should()
//                 .BeEquivalentTo(response);
//         }

//         [Fact]
//         public async Task GetAsync_ShouldNotGetUser_WhenNotExistsUser()
//         {
//             //Arrange
//             var request = new GetUserRequestViewModel() { Id = new Guid() };

//             _userRepository
//                 .Get(It.IsAny<Guid>())
//                 .Returns(It.IsAny<UserEntity>());

//             //Act
//             var result = await _userHandler.GetAsync(request);

//             //Assertion
//             result.IsFailure
//                    .Should()
//                    .BeTrue();

//             result.Error?
//                 .Message
//                 .Should()
//                 .Be("User not found");
//         }


//         [Fact]
//         public async Task LoginAsync_ShouldLogin_WhenRequestIsValid()
//         {
//             //Arrange
//             var request = new LoginUserRequestViewModel()
//             {
//                 Email = "teste@teste.com",
//                 Password = "teste123"
//             };

//             var tokenResponse = new LoginUserResponseViewModel
//             {
//                 Token = "token123Generated"
//             };

//             var userEntity = UserEntity.Create("Teste", "1", request.Email, request.Password).Value;

//             _userRepository
//                 .GetByEmailAsync(request.Email)
//                 .Returns(userEntity);

//             _authenticationService
//                 .IsPasswordCorrect(request.Password, userEntity.Password)
//                 .Returns(true);

//             _authenticationService
//                 .GenerateToken(Arg.Any<UserEntity>())
//                 .Returns(tokenResponse.Token);

//             //Act
//             var result = await _userHandler.LoginAsync(request);

//             //Assertion
//             _authenticationService
//                 .Received(1)
//                 .GenerateToken(userEntity);

//             result.IsSuccess
//                 .Should()
//                 .BeTrue();

//             result.Value
//                 .Should()
//                 .BeEquivalentTo(tokenResponse);
//         }

//         [Fact]
//         public async Task LoginAsync_ShouldNotLogin_WhenEmailIsEmpty()
//         {
//             //Arrange
//             var request = new LoginUserRequestViewModel()
//             {
//                 Email = "",
//                 Password = "teste123"
//             };

//             //Act
//             var result = await _userHandler.LoginAsync(request);

//             //Assertion
//             result.IsFailure
//                 .Should()
//                 .BeTrue();

//             result.Error?.Message
//                 .Should()
//                 .Be("Empty email");
//         }

//         [Fact]
//         public async Task LoginAsync_ShouldNotLogin_WhenPasswordlIsEmpty()
//         {
//             //Arrange
//             var request = new LoginUserRequestViewModel()
//             {
//                 Email = "teste@teste.com",
//                 Password = ""
//             };

//             //Act
//             var result = await _userHandler.LoginAsync(request);

//             //Assertion
//             result.IsFailure
//                 .Should()
//                 .BeTrue();

//             result.Error?.Message
//                 .Should()
//                 .Be("Empty password");
//         }

//         [Fact]
//         public async Task LoginAsync_ShouldNotLogin_WhenRequestIsValid()
//         {
//             //Arrange
//             var request = new LoginUserRequestViewModel()
//             {
//                 Email = "teste@teste.com",
//                 Password = "teste123"
//             };

//             _userRepository
//                  .GetByEmailAsync(It.IsAny<string>())
//                  .Returns(It.IsAny<UserEntity>());

//             //Act
//             var result = await _userHandler.LoginAsync(request);

//             //Assertion
//             result.IsFailure
//                  .Should()
//                  .BeTrue();

//             result.Error?.Message
//                 .Should()
//                 .Be("User not found");
//         }

//         [Fact]
//         public async Task LoginAsync_ShouldNotLogin_WhenPasswordIsWrong()
//         {
//             //Arrange
//             var request = new LoginUserRequestViewModel()
//             {
//                 Email = "teste@teste.com",
//                 Password = "teste123"
//             };

//             var userEntity = UserEntity.Create("Teste", "1", request.Email, request.Password).Value;

//             _userRepository
//                 .GetByEmailAsync(request.Email)
//                 .Returns(userEntity);

//             _authenticationService
//                 .IsPasswordCorrect(request.Password, userEntity.Password)
//                 .Returns(false);

//             //Act
//             var result = await _userHandler.LoginAsync(request);

//             //Assertion
//             result.IsFailure
//                 .Should()
//                 .BeTrue();

//             result.Error?.Message
//                 .Should()
//                 .Be("Wrong password");
//         }
//     }
// }
