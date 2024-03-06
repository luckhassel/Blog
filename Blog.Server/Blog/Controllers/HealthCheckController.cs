using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Route("api")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly string _environment;

        public HealthCheckController()
        {
            _environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "No environment set";            
        }

        [HttpGet]
        public ActionResult<string> HealthCheck()
        {
            return Ok(_environment);
        }
    }
}
