using Microsoft.AspNetCore.Http.HttpResults;
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
    public class AController : ControllerBase
    {

        private readonly ApplicationContext _context;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<AuthController> _logger;

        public AController(ILogger<AuthController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

    }
}
