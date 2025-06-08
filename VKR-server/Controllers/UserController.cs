using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using VKR_server.DB;
using VKR_server.DB.Entities;
using VKR_server.Dto;
using VKR_server.JWT;

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
        public IActionResult GetUsers()
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var users = _context.Users.Select(u => new UserResponseDto
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                Patronymic = u.Patronymic,
                LastName = u.LastName,
                Email = u.Email,
                GroupName = u.Group.GroupName,
                CountWorks = u.Files.Count(),
                RoleName = u.Role.RoleName,
            });
            return Ok(users);
        }

        [HttpGet("user/{user_id}", Name = "GetUser")]
        [Authorize]
        public IActionResult GetUser(int user_id)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            //Include явно загружает связанные данные
            var user = _context.Users.Include(u => u.Role).Include(u => u.Group).FirstOrDefault(u => u.Id == user_id);

            if (user == null) return BadRequest("Пользователь не существует");
            return Ok(new UserResponseDto
            {
                UserId = user_id,
                Email = user.Email,
                FirstName = user.FirstName,
                Patronymic = user.Patronymic,
                LastName = user.LastName,
                CountWorks = user.Files.Count(),
                RoleName = user.Role.RoleName,
                GroupName = user.Group?.GroupName,
            });
        }
        [HttpGet("roles", Name = "GetRoles")]
        [Authorize]
        public IActionResult GetRoles()
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var roles = _context.Roles.ToList();
            return Ok(roles);
        }

        [HttpGet("users-on-role/{role_id}", Name = "GetUsersOnRole")]
        [Authorize]
        public IActionResult GetUsersOnRole(int role_id)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            //Select по умолчанию неявно подгружает связанные сущности
            var users = _context.Users.Where(u => u.RoleId == role_id).Select(s => new UserResponseDto
            {
                UserId = s.Id,
                Email = s.Email,
                FirstName = s.FirstName,
                Patronymic = s.Patronymic,
                LastName = s.LastName,
                RoleName = s.Role.RoleName,
                CountWorks = s.Files.Count(),
                GroupName = s.Group.GroupName
            });
            return Ok(users);
        }

        [HttpGet("students/{groupId}", Name = "GetStudents")]
        [Authorize]
        public IActionResult GetStudents(int groupId)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName == "Student") return BadRequest("Invalide role");

            //var students = GetUserByRole("Student");
            var studentsInGroup = _context.Users.Where(s => s.GroupId == groupId);
            var studentsInGroupDto = studentsInGroup.Select(s => new UserResponseDto
            {
                UserId = s.Id,
                Email = s.Email,
                FirstName = s.FirstName,
                Patronymic = s.Patronymic,
                LastName = s.LastName,
                RoleName = s.Role.RoleName,
                CountWorks = s.Files.Count(),
                GroupName = s.Group.GroupName
            });
            return Ok(studentsInGroupDto);
        }

        [HttpGet("groups", Name = "GetGroups")]
        [Authorize]
        public IActionResult GetGroups()
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName == "Student") return BadRequest("Invalide role");

            var groups = _context.Groups.Where(g => g.Users.ToList().Count != 0).ToList();

            return Ok(groups);
        }

        [HttpPut("update-role/{id}/{idNewRole}", Name = "UpdateRoleUser")]
        [Authorize]
        public IActionResult UpdateRoleUser(int id, int idNewRole)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);
            
            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var user = _context.Users.Find(id);
            if (user == null) return BadRequest("Пользователя не существует");
            
            user.RoleId = idNewRole;
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok(id);
        }

        [HttpDelete("delete-user/{user_id}", Name = "DeleteUser")]
        [Authorize]
        public IActionResult DeleteUser(int user_id)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            var user = _context.Users.Find(user_id);
            if (user == null) return BadRequest("Пользователя не существует");

            var files = _context.Files.Where(f => f.UserId == user_id);
            _context.Users.Remove(user);
            foreach (var file in files)
            {
                _context.Files.Remove(file);
            }
            _context.SaveChanges();
            
            return Ok(user_id);
        }

        private JwtData GetJwtData(string token)
        {
            var stream = HttpContext.Request.Headers.Authorization.ToString().Split()[1];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(stream);

            return new JwtData()
            {
                Email = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "Email")!.Value.ToString(),
                FirstName = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "FirstName")!.Value.ToString(),
                Patronymic = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "Patronymic")?.Value.ToString(),
                LastName = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "LastName")!.Value.ToString(),
                RoleName = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "RoleName")!.Value.ToString()
            };
        }

    }
}
