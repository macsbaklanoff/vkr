using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VKR_server.DB;
using VKR_server.Dto;
using VKR_server.JWT;
using Microsoft.SemanticKernel;
using Together;
using Together.Models.ChatCompletions;
using Together.SemanticKernel.Extensions;

namespace VKR_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly ILogger<FileController> _logger;

        private readonly ApplicationContext _context;

        private Kernel _kernel;

        public FileController(ILogger<FileController> logger, ApplicationContext context)
        {
            _logger = logger;
            _context = context;
            
        }
        //meta - llama / Llama - 3.3 - 70B - Instruct - Turbo - Free
        //meta-llama/Llama-Vision-Free
        //a058cfce7db8aed599e95914b94e9999403a6392d87bdfc444023920bd1671ed
        [HttpPost("upload-file", Name = "UploadFile")]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileDto uploadFile)
        {

            var client = new TogetherClient("a058cfce7db8aed599e95914b94e9999403a6392d87bdfc444023920bd1671ed");
            var response = await client.ChatCompletions.CreateAsync(new ChatCompletionRequest
            {
                Model = "meta-llama/Llama-Vision-Free",
                Messages = new[]
                {
                    new ChatCompletionMessage { Role = "user", Content = "Hello!" }
                }
            });
            return Ok();
        }
    }
}
