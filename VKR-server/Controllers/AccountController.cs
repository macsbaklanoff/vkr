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

        [HttpPost("sign-in", Name = "Token")]
        public IActionResult SignIn(UserDtoAuth userDtoAuth)
        {
            var claimsIdentity = GetIdentity(userDtoAuth.Email, userDtoAuth.Password);
            
            if (claimsIdentity == null)
            {
                return BadRequest("User dont exists");
            }

            var jwt = GetToken(claimsIdentity);
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                accessToken = encodedJwt,
                userEmail = userDtoAuth.Email,
            };

            return Ok(response);
        }

        [HttpPost("sign-up", Name = "SignUp")]
        public IActionResult SignUp(UserDto userDto)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == userDto.Email);
            if (user != null)
            {
                return BadRequest("User with this email exist");
            }

            var new_user = new User
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Password = userDto.Password,
                RoleId = 2
            };

            _context.Users.Add(new_user);
            _context.SaveChanges();

            ClaimsIdentity claimsIdentity = GetClaims(new_user);
            var jwt = GetToken(claimsIdentity);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                accessToken = encodedJwt,
                userEmail = new_user.Email,
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

        private ClaimsIdentity? GetIdentity(string email, string password)
        {
            User? user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);


            if (user != null)
            {
                return GetClaims(user);
            }
            return null;
        }

        private ClaimsIdentity GetClaims(User user)
        {
            var userRole = _context.Roles.FirstOrDefault(uR => uR.Id == user.RoleId);
            var claims = new List<Claim>
                {
                    new Claim("Email", user.Email),
                    new Claim("Name", $"{user.FirstName} + {user.LastName}"),
                    new Claim("RoleName", userRole.RoleName)
                };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
