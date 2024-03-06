using Blog.Application.ViewModels;
using Blog.Domain.Models.Shared;

namespace Blog.Application.Interfaces
{
    public interface INewsHandler
    {
        Task<Result<CreateNewsResponseViewModel>> Create(CreateNewsRequestViewModel request);
        Task<Result<GetNewsResponseViewModel>> Get(GetNewsRequestViewModel request);
        Result<IReadOnlyList<GetNewsResponseViewModel>> List();
    }
}
