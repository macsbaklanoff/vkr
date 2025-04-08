using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VKR_server.DB;
using VKR_server.DB.Entities;
using VKR_server.Dto;

namespace VKR_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly ApplicationContext _context;

        public AccountController(ILogger<AuthController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<UserDto> Get()
        {
            var users = _context.Users.ToList();
            var users_dto = users.Select(u => new UserDto()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                RoleId = u.RoleId,
            });
            return users_dto;
        }
        [HttpPost("sign-up", Name = "RegisterUser")]
        public IActionResult Register(UserDto user)
        {
            var users = _context.Users.ToList();

            var new_user = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                Email = user.Email,
                RoleId = user.RoleId,
                CreationDate = DateTime.UtcNow
            };
            _context.Users.Add(new_user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Register), new_user);
        }
        [HttpPost("sign-in", Name = "LoginUser")]
        public IActionResult GetIdentity(UserDtoLogin user)
        {
            var users = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            if (users != null)
            {
                return BadRequest("User email already exist");
            }
            return Ok(user);
        }


    }
}
