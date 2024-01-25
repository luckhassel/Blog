using Blog.Application.Interfaces;
using Blog.Application.Settings;
using Blog.Application.ViewModels;
using Blog.Domain.Entities;
using Blog.Domain.Enums;
using Blog.Domain.Interfaces.Repositories;
using Blog.Domain.Interfaces.Services;
using Blog.Domain.Messages;
using Blog.Domain.Models.Shared;
using MassTransit;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Blog.Application.Handlers
{
    public class NewsHandler : INewsHandler
    {
        private readonly INewsRepository _newsRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IUserRepository _userRepository;
        private readonly IMessageBrokerService _messageBrokerService;
        private readonly ApplicationSettings _applicationSettings;
        public NewsHandler(
            INewsRepository newsRepository, 
            IHttpContextAccessor httpContext, 
            IUserRepository userRepository,
            IMessageBrokerService messageBrokerService,
            ApplicationSettings applicationSettings)
        {
            _newsRepository = newsRepository;
            _httpContext = httpContext;
            _userRepository = userRepository;
            _messageBrokerService = messageBrokerService;
            _applicationSettings = applicationSettings;
        }

        public async Task<Result<CreateNewsResponseViewModel>> Create(CreateNewsRequestViewModel request)
        {
            var authorEmail = _httpContext.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrWhiteSpace(authorEmail))
                return Result.Failure<CreateNewsResponseViewModel>(Error.Create(3, "Author email not found"));

            var message = new CreateNewsMessage()
            {
                Title = request.Title, 
                Description = request.Description, 
                AuthorEmail = authorEmail
            };

            var author = await _userRepository.GetByEmailAsync(authorEmail);

            if (author is null)
                return Result.Failure<CreateNewsResponseViewModel>(Error.Create(3, "Author not found"));

            var news = NewsEntity.Create(request.Title, request.Description, author);

            if (news.IsFailure)
                return Result.Failure<CreateNewsResponseViewModel>(news.Error);

            await _newsRepository.Create(news.Value);
            await _newsRepository.Commit();

            var sendResult = await _messageBrokerService.Send(BrokerTypes.Queue, _applicationSettings.MassTransitSettings.NewsQueue, message);

            if (sendResult.IsFailure)
                return Result.Failure<CreateNewsResponseViewModel>(sendResult.Error);

            return new CreateNewsResponseViewModel { Id = news.Value.Guid };
        }

        public async Task<Result<GetNewsResponseViewModel>> Get(GetNewsRequestViewModel request)
        {
            var result = await _newsRepository.Get(request.Id);

            if (result is null)
                return Result.Failure<GetNewsResponseViewModel>(Error.Create(3, "news not found"));

            return new GetNewsResponseViewModel 
            { 
                Author = new GetUserResponseViewModel { Email = result.Author.Email, FirstName = result.Author.FirstName, LastName = result.Author.LastName, Id = result.Author.Guid },
                Description = result.Description,
                Title = result.Title,
                Id = result.Guid
            };
        }

        public Result<IReadOnlyList<GetNewsResponseViewModel>> List()
        {
            var newsList = _newsRepository.List();

            if (newsList is null || !newsList.Any())
                return Result.Failure<IReadOnlyList<GetNewsResponseViewModel>>(Error.Create(3, "news not found"));

            var result = new List<GetNewsResponseViewModel>();

            foreach (var news in newsList)
            {
                result.Add(
                    new GetNewsResponseViewModel
                    {
                        Author = new GetUserResponseViewModel { Email = news.Author.Email, FirstName = news.Author.FirstName, LastName = news.Author.LastName, Id = news.Author.Guid },
                        Description = news.Description,
                        Title = news.Title,
                        Id = news.Guid
                    }
                );
            }

            return result;
        }
    }
}
