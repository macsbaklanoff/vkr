using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations;
using System.ComponentModel.DataAnnotations;
using VKR_server.DB;
using VKR_server.DB.Entities;
using VKR_server.Dto;

namespace VKR_server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ApplicationContext _context;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost("add", Name = "AddUser")]
        public IActionResult Add(UserDto user)
        {
            var new_user = new User()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = user.Password,
                RoleId = user.RoleId,
                CreationDate = DateTime.UtcNow
            };
            _context.Users.Add(new_user);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Add), new_user);
        }
    }
}
