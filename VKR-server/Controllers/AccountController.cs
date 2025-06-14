using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.RegularExpressions;
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

		private readonly IPasswordHasher<User> _passwordHasher;

		public AuthController(ILogger<AuthController> logger, ApplicationContext context, IPasswordHasher<User> passwordHasher)
		{
			_logger = logger;
			_context = context;
			_passwordHasher = passwordHasher;
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

			if (userDto.GroupName != null)
			{
				var group = _context.Groups.FirstOrDefault(g => g.GroupName == userDto.GroupName);
				if (group == null)
				{
					_context.Groups.Add(new DB.Entities.Group { GroupName = userDto.GroupName });
					_context.SaveChanges();
					group = _context.Groups.FirstOrDefault(g => g.GroupName == userDto.GroupName);
				}
				new_user = new User
				{
					Email = userDto.Email,
					FirstName = userDto.FirstName,
					Patronymic = userDto.Patronymic,
					LastName = userDto.LastName,
					Password = userDto.Password,
					RoleId = 3,
					GroupId = group!.GroupId
				};
			}
            new_user = new User
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                Patronymic = userDto.Patronymic,
                LastName = userDto.LastName,
                Password = userDto.Password,
                RoleId = 3,
            };
			string hashedPassword = _passwordHasher.HashPassword(new_user, userDto.Password);
			new_user.Password = hashedPassword;
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
				if (updateUser.Email == string.Empty) return BadRequest("Email не может быть пустым!");
                var matchEmail = Regex.Match(updateUser.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
				if (!matchEmail.Success) return BadRequest($"{updateUser.Email}");
                var users = _context.Users.ToList();
				if (!FindCountEmail(users, updateUser.Email))
				{
					return BadRequest("Email уже существует!");
				}
				user.Email = updateUser.Email;
			}
			if (updateUser.FirstName == string.Empty || updateUser.LastName == string.Empty) //условия на обновления имен
			{
				return BadRequest("Имя или Фамилия не могут быть пустыми");
			}
			user.FirstName = updateUser.FirstName;
			user.LastName = updateUser.LastName;
			user.Patronymic = updateUser.Patronymic;

			try
			{
				if (updateUser.GroupName != null) //условия на обновления группы
				{
					var group = _context.Groups.FirstOrDefault(u => u.GroupName == updateUser.GroupName);
					if (group == null)
					{
						_context.Groups.Add(new DB.Entities.Group { GroupName = updateUser.GroupName });
						_context.SaveChanges();
						group = _context.Groups.FirstOrDefault(u => u.GroupName == updateUser.GroupName);
					}
					user.GroupId = group.GroupId;
				}
				else
				{
                    user.GroupId = null;
                }
                _context.SaveChanges();
			} catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			if (updateUser.GroupName == null) user.GroupId = null;
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
			User? user = _context.Users.FirstOrDefault(u => u.Email == email);

			if (user == null) return null;


			var test = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

			if (test == PasswordVerificationResult.Success)
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
					new Claim("CreatedAt", $"{user.CreationDate.ToString()}"),
				};
			if (userGroup != null)
			{
				claims.Add(new Claim("GroupName", userGroup.GroupName));
			}
			if (user.Patronymic != null)
			{
				claims.Add(new Claim("Patronymic", user.Patronymic));
			}
			ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
				ClaimsIdentity.DefaultRoleClaimType);
			return claimsIdentity;
		}
	}
}
