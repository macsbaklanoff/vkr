using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VKR_server.DB;
using VKR_server.Dto;
using VKR_server.JWT;

namespace VKR_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        private readonly ApplicationContext _context;

        public FileController(ILogger<FileController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpPost("upload-file", Name = "UploadFile")]
        [Authorize]
        public IActionResult UploadFile([FromForm] UploadFileDto uploadFile)
        {
            Console.WriteLine(uploadFile.File.Name);
            return Ok();
        }
    }
}
