using Blog.Application.ViewModels;
using Blog.Domain.Models.Shared;

namespace Blog.Application.Interfaces
{
    public interface IUserHandler
    {
        Task<Result<GetUserResponseViewModel>> GetAsync(GetUserRequestViewModel request);
        Task<Result<CreateUserResponseViewModel>> CreateAsync(CreateUserRequestViewModel request);
        Task<Result<LoginUserResponseViewModel>> LoginAsync(LoginUserRequestViewModel request);
    }
}
