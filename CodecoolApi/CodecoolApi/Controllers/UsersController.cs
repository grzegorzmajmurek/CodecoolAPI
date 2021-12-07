using CodecoolApi.DAL.DTO.User;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CodecoolApi.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly ILogger<MaterialsController> _logger;

        public UsersController(ILogger<MaterialsController> logger, IUserRepository userRepo)
        {
            _logger = logger;
            _userRepo = userRepo;
        }

        [SwaggerOperation(Summary = "Authenticate user")]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationDto model)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            var user = _userRepo.Authenticate(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorect" });
            }
            _logger.LogInformation($"Return {user} of {user.GetType()}");
            return Ok(user);
        }

        [SwaggerOperation(Summary = "Register user")]
        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationDto model)
        {
            _logger.LogInformation($"Enter {HttpContext.Request.Path}{HttpContext.Request.QueryString}");
            bool isUserNameUnique = _userRepo.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
            {
                return BadRequest(new { message = "Username already exist" });
            }
            var user = _userRepo.Register(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Error while registerig" });
            }
            _logger.LogInformation($"Return {user} of {user.GetType()}");
            return Ok();
        }
    }
}