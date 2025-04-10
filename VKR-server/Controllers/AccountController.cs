using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VKR_server.DB;
using VKR_server.DB.Entities;
using VKR_server.Dto;
using VKR_server.JWT;

namespace VKR_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;

        private readonly ApplicationContext _context;

        public AuthController(ILogger<AuthController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetUser")]
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
        [HttpPost("sign-in", Name = "Token")]
        public IActionResult SignIn(UserDtoLogin userDtoLogin)
        {
            var user = GetIdentity(userDtoLogin.Email, userDtoLogin.Password);
            
            if (user == null)
            {
                return BadRequest("User dont exists");
            }

            var jwt = GetToken(user);
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                accessToken = encodedJwt,
                userEmail = userDtoLogin.Email,
            };

            return Ok(response);
        }

        private JwtSecurityToken GetToken(ClaimsIdentity user)
        {
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: DateTime.UtcNow,
                claims: user.Claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return jwt;
        }

        private ClaimsIdentity GetIdentity(string email, string password)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);


            if (user != null)
            {
                var userRole = _context.Roles.FirstOrDefault(uR => uR.Id == user.RoleId);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Name, $"{user.FirstName} + {user.LastName}"),
                    new Claim(ClaimTypes.Role, userRole.RoleName)
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }
    }
}
