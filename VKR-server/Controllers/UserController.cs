using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
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

            var users = GetUserByRole("");
            return Ok(users);
        }

        [HttpGet("admins", Name = "GetAdmins")]
        [Authorize]
        public IActionResult GetAdmins()
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var admins = GetUserByRole("Admin");

            return Ok(admins);
        }

        [HttpGet("teachers", Name = "GetTeachers")]
        [Authorize]
        public IActionResult GetTeachers()
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var teachers = GetUserByRole("Teacher");

            return Ok(teachers);
        }

        [HttpGet("students", Name = "GetStudents")]
        [Authorize]
        public IActionResult GetStudents()
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);

            if (jwt.RoleName == "Student") return BadRequest("Invalide role");

            var students = GetUserByRole("Student");

            return Ok(students);
        }

        [HttpPut("update-role/{id}/{idNewRole}", Name = "UpdateRoleUser")]
        [Authorize]
        public IActionResult UpdateRoleUser(int id, int roleId)
        {
            var jwt = GetJwtData(HttpContext.Request.Headers.Authorization.ToString().Split()[1]);
            
            if (jwt.RoleName != "Admin") return BadRequest("Invalide role");

            var user = _context.Users.Find(id);
            if (user == null) return BadRequest("Not-existent user");
            
            user.RoleId = roleId;

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
        private IEnumerable<UserDto> GetUserByRole(string roleName) 
        {
            var users = _context.Users.ToList();
            var new_users = users.Select(u => new UserDto()
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Password = u.Password,
                RoleId = u.RoleId,
                RoleName = _context.Roles.ToList().FirstOrDefault(r => r.Id == u.RoleId)?.RoleName.ToString()
            }).Where(u => u.RoleName.Contains(roleName));

            return new_users;
        }
    }
}
