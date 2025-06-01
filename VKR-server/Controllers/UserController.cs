using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                LastName = u.LastName,
                Email = u.Email,
                GroupName = _context.Groups.FirstOrDefault(g => g.GroupId == u.GroupId).GroupName,
                CountWorks = u.Files.Count(),
                RoleName = _context.Roles.FirstOrDefault(r => r.RoleId == u.RoleId).RoleName,
            });
            return Ok(users);
        }

        [HttpGet("user/{user_id}", Name = "GetUser")]
        [Authorize]
        public IActionResult GetUser(int user_id)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var user = _context.Users.FirstOrDefault(u => u.Id == user_id);
            if (user == null) return BadRequest("User doesnt exists");
            return Ok(new UserResponseDto
            {
                UserId = user_id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CountWorks = user.Files.Count(),
                RoleName = "Student",
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
        public IActionResult GetRoles(int role_id)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var users = _context.Users.Where(u => u.RoleId == role_id).Select(s => new UserResponseDto
            {
                UserId = s.Id,
                Email = s.Email,
                FirstName = s.FirstName,
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
            if (user == null) return BadRequest("Not-existent user");
            
            user.RoleId = idNewRole;
            _context.Users.Update(user);
            _context.SaveChanges();
            return Ok(id);
        }

        [HttpDelete("delete-user/{id}", Name = "DeleteUser")]
        [Authorize]
        public IActionResult DeleteUser(int id)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var user = _context.Users.Find(id);
            if (user == null) return BadRequest("Not-existent user");

            _context.Users.Remove(user);
            _context.SaveChanges();

            return Ok(id);
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
                LastName = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "LastName")!.Value.ToString(),
                RoleName = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "RoleName")!.Value.ToString()
            };
        }

    }
}
