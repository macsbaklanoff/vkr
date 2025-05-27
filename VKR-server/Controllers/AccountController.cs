using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VKR_server.Models.DB;
using VKR_server.Models.DB.Entities;
using VKR_server.Models.Dto;
using VKR_server.Options;

namespace VKR_server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ILogger<AuthController> logger, ApplicationContext context, IOptions<JwtOptions> jwtOptions)
    : ControllerBase
{
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
            userEmail = userDtoAuth.Email
        };

        return Ok(response);
    }

    [HttpPost("sign-up", Name = "SignUp")]
    public IActionResult SignUp(UserDto userDto)
    {
        var user = context.Users.FirstOrDefault(u => u.Email == userDto.Email);
        if (user != null)
        {
            return BadRequest("User with this email exist");
        }

        var new_user = new User();

        if (userDto.GroupName == null)
        {
            new_user = new User
            {
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                Password = userDto.Password,
                RoleId = 3
            };
            context.Users.Add(new_user);
            context.SaveChanges();
        }

        if (userDto.GroupName != null)
        {
            var group = context.Groups.FirstOrDefault(g => g.GroupName == userDto.GroupName);
            if (group == null)
            {
                context.Groups.Add(new Group { GroupName = userDto.GroupName });
                context.SaveChanges();
                group = context.Groups.FirstOrDefault(g => g.GroupName == userDto.GroupName);
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
            context.Users.Add(new_user);
            context.SaveChanges();
        }

        var claimsIdentity = GetClaims(new_user);
        var jwt = GetToken(claimsIdentity);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            accessToken = encodedJwt,
            userEmail = new_user.Email
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPut("update-user-data", Name = "UpdateUserData")]
    public IActionResult UpdateUserData(UpdateUserDto updateUser)
    {
        var user = context.Users.FirstOrDefault(u => u.Id == updateUser.UserId);
        if (user == null)
        {
            return BadRequest("User doesnt exists");
        }

        if (user.Email != updateUser.Email) //условия на обновления email
        {
            if (updateUser.Email == string.Empty)
            {
                return BadRequest("Email is empty!");
            }

            var users = context.Users.ToList();
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
            var group = context.Groups.FirstOrDefault(u => u.GroupName == updateUser.GroupName);
            if (group == null)
            {
                context.Groups.Add(new Group { GroupName = updateUser.GroupName });
                context.SaveChanges();
                group = context.Groups.FirstOrDefault(u => u.GroupName == updateUser.GroupName);
            }

            user.GroupId = group.GroupId;
        }

        if (updateUser.GroupName == null)
        {
            user.GroupId = null;
        }

        context.SaveChanges();
        var claimsIdentity = GetClaims(user);
        var jwt = GetToken(claimsIdentity);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            accessToken = encodedJwt,
            userEmail = user.Email
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
            jwtOptions.Value.Issuer,
            jwtOptions.Value.Audience,
            notBefore: DateTime.UtcNow,
            claims: user.Claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(jwtOptions.Value.Lifetime)),
            signingCredentials: new SigningCredentials(jwtOptions.Value.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return jwt;
    }

    private ClaimsIdentity? GetIdentity(string email, string password)
    {
        var user = context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

        if (user != null)
        {
            return GetClaims(user);
        }

        return null;
    }

    private ClaimsIdentity GetClaims(User user)
    {
        var userRole = context.Roles.FirstOrDefault(uR => uR.RoleId == user.RoleId);
        var userGroup = context.Groups.FirstOrDefault(uG => uG.GroupId == user.GroupId);
        var claims = new List<Claim>
        {
            new("UserId", $"{user.Id}"),
            new("Email", user.Email),
            new("FirstName", $"{user.FirstName}"),
            new("LastName", $"{user.LastName}"),
            new("RoleName", userRole.RoleName)
        };
        if (userGroup != null)
        {
            claims.Add(new Claim("GroupName", userGroup.GroupName));
        }

        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
        return claimsIdentity;
    }
}
