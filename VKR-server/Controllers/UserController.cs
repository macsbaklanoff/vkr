using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VKR_server.DB;
using VKR_server.Dto;

namespace VKR_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        private readonly ApplicationContext _context;

        public UserController(ILogger<UserController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("users", Name = "GetUsers")]
        [Authorize]
        public IActionResult Get()
        {
            Console.WriteLine(HttpContext.Request.Headers.Authorization);

            var stream = HttpContext.Request.Headers.Authorization.ToString().Split()[1];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(stream);
            var role = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "RoleName")?.Value.ToString();

            if (role != "Admin") return BadRequest("Invalide role");


            var users = _context.Users.ToList();
            var users_dto = users.Select(u => new UserDto()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                RoleId = u.RoleId,
            });
            return Ok(users_dto);
        }
    }
}
