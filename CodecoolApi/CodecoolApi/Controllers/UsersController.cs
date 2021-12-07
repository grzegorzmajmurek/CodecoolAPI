using CodecoolApi.DAL.DTO.User;
using CodecoolApi.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodecoolApi.Controllers
{
    [Authorize]
    [Route("api/Users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UsersController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticationDto model)
        {
            var user = _userRepo.Authenticate(model.UserName, model.Password);
            if (user == null)
            {
                return BadRequest(new { message = "Username or password is incorect" });
            }
            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] AuthenticationDto model)
        {
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
            return Ok();
        }
    }
}