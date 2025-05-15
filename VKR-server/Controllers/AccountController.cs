using Microsoft.AspNetCore.Authorization;
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

            User new_user = new User();

            if (userDto.GroupName == null)
            {
                new_user = new User
                {
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Password = userDto.Password,
                    RoleId = 3,
                };
                _context.Users.Add(new_user);
                _context.SaveChanges();
            }

            if (userDto.GroupName != null)
            {
                var group = _context.Groups.FirstOrDefault(g => g.GroupName == userDto.GroupName);
                if (group == null)
                {
                    _context.Groups.Add(new Group { GroupName = userDto.GroupName });
                    _context.SaveChanges();
                    group = _context.Groups.FirstOrDefault(g => g.GroupName == userDto.GroupName);
                }
                new_user = new User
                {
                    Email = userDto.Email,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Password = userDto.Password,
                    RoleId = 3,
                    GroupId = group!.GroupId
                };
                _context.Users.Add(new_user);
                _context.SaveChanges();
            }

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

        [HttpPut("update-user-data", Name = "UpdateUserData")]
        [Authorize]
        public IActionResult UpdateUserData(UpdateUserDto updateUser)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == updateUser.UserId);
            if (user == null)
            {
                return BadRequest("User doesnt exists");
            }
            if (user.Email != updateUser.Email) //условия на обновления email
            {
                var users = _context.Users.ToList();
                if (!FindCountEmail(users, updateUser.Email))
                {
                    return BadRequest("Email already exists!");
                }
                user.Email = updateUser.Email;
            }

            if (updateUser.FirstName == "" || updateUser.LastName == "") //условия на обновления имен
            {
                return BadRequest("FirstName or LastName is empty");
            }
            user.FirstName = updateUser.FirstName;
            user.LastName = updateUser.LastName;

            if (updateUser.GroupName != null) //условия на обновления группы
            {
                var group = _context.Groups.FirstOrDefault(u => u.GroupName == updateUser.GroupName);
                if (group == null)
                {
                    _context.Groups.Add(new Group { GroupName = updateUser.GroupName });
                    group = _context.Groups.FirstOrDefault(u => u.GroupName == updateUser.GroupName);
                }
                user.GroupId = group.GroupId;
            }
            if (updateUser.GroupName == null) user.GroupId = null;

            _context.SaveChanges();
            ClaimsIdentity claimsIdentity = GetClaims(user);
            var jwt = GetToken(claimsIdentity);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                accessToken = encodedJwt,
                userEmail = user.Email,
            };

            return Ok(response);
        }
        private bool FindCountEmail(List<User> users, string newEmail)
        {
            return users.All(user => user.Email != newEmail);
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
            var userRole = _context.Roles.FirstOrDefault(uR => uR.RoleId == user.RoleId);
            var userGroup = _context.Groups.FirstOrDefault(uG => uG.GroupId == user.GroupId);
            var claims = new List<Claim>
                {
                    new Claim("UserId",$"{user.Id}"),
                    new Claim("Email", user.Email),
                    new Claim("FirstName", $"{user.FirstName}"),
                    new Claim("LastName", $"{user.LastName}"),
                    new Claim("RoleName", userRole.RoleName),
                };
            if (userGroup != null)
            {
                claims.Add(new Claim("GroupName", userGroup.GroupName));
            }
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
