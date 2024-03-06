using Blog.Application.Interfaces;
using Blog.Application.ViewModels;
using Blog.Domain.Models.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly ILogger<NewsController> _logger;
        private readonly INewsHandler _newsHandler;

        public NewsController(ILogger<NewsController> logger, INewsHandler newsHandler)
        {
            _logger = logger;
            _newsHandler = newsHandler;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Result>> CreateNews([FromBody] CreateNewsRequestViewModel request)
        {
            var result = await _newsHandler.Create(request);

            return result.IsSuccess ? Created(nameof(GetNewsById), new { id = result.Value.Id }) : UnprocessableEntity();
        }

        [HttpGet("{id}", Name = nameof(GetNewsById))]
        public async Task<ActionResult<Result<GetNewsResponseViewModel>>> GetNewsById(Guid id)
        {
            var request = new GetNewsRequestViewModel { Id = id };
            var result = await _newsHandler.Get(request);

            return result.IsSuccess ? Ok(result) : NotFound();
        }

        [HttpGet]
        public ActionResult<Result<IReadOnlyList<GetNewsResponseViewModel>>> ListNews()
        {
            var result = _newsHandler.List();

            return result.IsSuccess ? Ok(result) : NotFound();
        }
    }
}
