using Blog.Application.Interfaces;
using Blog.Application.ViewModels;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;
using Blog.Domain.Models.Shared;

namespace Blog.Application.Handlers
{
    public class UserHandler : IUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        public UserHandler(IUserRepository userRepository, IAuthenticationService authenticationService)
        {
            _userRepository = userRepository;
            _authenticationService = authenticationService;
        }

        public async Task<Result<CreateUserResponseViewModel>> CreateAsync(CreateUserRequestViewModel request)
        {
            var userByEmail = await _userRepository.GetByEmailAsync(request.Email);

            if (userByEmail is not null)
                return Result.Failure<CreateUserResponseViewModel>(Error.Create(2, "Email already in use"));

            var passwordHashed = _authenticationService.HashPassword(request.Password);
            var user = UserEntity.Create(request.FirstName, request.LastName, request.Email, passwordHashed);

            if (user.IsSuccess)
            {
                await _userRepository.Create(user.Value);
                await _userRepository.Commit();
            }

            return new CreateUserResponseViewModel { Id = user.Value.Guid };
        }

        public async Task<Result<GetUserResponseViewModel>> GetAsync(GetUserRequestViewModel request)
        {
            var user = await _userRepository.Get(request.Id);

            if (user is null)
            {
                return Result.Failure<GetUserResponseViewModel>(Error.Create(2, "User not found"));
            }

            return new GetUserResponseViewModel
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Id = user.Guid
            };
        }

        public async Task<Result<LoginUserResponseViewModel>> LoginAsync(LoginUserRequestViewModel request)
        {
            if (string.IsNullOrWhiteSpace(request.Email))
                return Result.Failure<LoginUserResponseViewModel>(Error.Create(2, "Empty email"));

            if (string.IsNullOrWhiteSpace(request.Password))
                return Result.Failure<LoginUserResponseViewModel>(Error.Create(2, "Empty password"));

            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
                return Result.Failure<LoginUserResponseViewModel>(Error.Create(2, "User not found"));

            var passwordMatch = _authenticationService.IsPasswordCorrect(request.Password, user.Password);

            if (passwordMatch is false)
                return Result.Failure<LoginUserResponseViewModel>(Error.Create(2, "Wrong password"));

            return new LoginUserResponseViewModel { Token = _authenticationService.GenerateToken(user) };
        }
    }
}
