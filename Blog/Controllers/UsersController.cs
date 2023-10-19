// using Blog.Application.Interfaces;
// using Blog.Application.ViewModels;
// using Blog.Domain.Models.Shared;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;

// namespace Blog.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class UsersController : ControllerBase
//     {
//         private readonly ILogger<UsersController> _logger;
//         private readonly IUserHandler _userHandler; 

//         public UsersController(ILogger<UsersController> logger, IUserHandler userHandler)
//         {
//             _logger = logger;
//             _userHandler = userHandler;
//         }

//         [HttpPost]
//         [AllowAnonymous]
//         public async Task<ActionResult<Result>> CreateUser([FromBody] CreateUserRequestViewModel request)
//         {
//             var result = await _userHandler.CreateAsync(request);

//             return result.IsSuccess ? Created(nameof(GetUserById), new { id = result.Value.Id }) : UnprocessableEntity(result); 
//         }

//         [HttpGet("{id}", Name = nameof(GetUserById))]
//         [Authorize]
//         public async Task<ActionResult<Result<GetUserResponseViewModel>>> GetUserById(Guid id)
//         {
//             var request = new GetUserRequestViewModel { Id = id };
//             var result = await _userHandler.GetAsync(request);

//             return result.IsSuccess ? Ok(result) : NotFound();
//         }

//         [HttpPost("login")]
//         [AllowAnonymous]
//         public async Task<ActionResult<Result<LoginUserResponseViewModel>>> LoginUser([FromBody] LoginUserRequestViewModel request)
//         {
//             var result = await _userHandler.LoginAsync(request);

//             return result.IsSuccess ? Ok(result) : UnprocessableEntity(result);
//         }
//     }
// }