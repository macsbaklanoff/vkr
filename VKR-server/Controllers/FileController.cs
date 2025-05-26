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

        [HttpPost("upload-file", Name = "UploadFile")]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromForm] UploadFileDto uploadFile)
        {

            Console.WriteLine(uploadFile.File.FileName);
            Console.WriteLine(uploadFile.UserId);
            //meta - llama / Llama - 3.3 - 70B - Instruct - Turbo - Free
            this._kernel = Kernel.CreateBuilder().AddTogetherChatCompletion(
                "meta-llama/Llama-Vision-Free",
                "a058cfce7db8aed599e95914b94e9999403a6392d87bdfc444023920bd1671ed"
                )
                .Build();
            //Task<string> content = ReadPdfFile(uploadFile.File);
            //Console.WriteLine(content.Result);
            var chatResult = await _kernel.InvokePromptAsync($"Привет!");
            Console.WriteLine(chatResult);
            return Ok();
        }
        //private async Task<string> ReadPdfFile(IFormFile file)
        //{
        //    using var memoryStream = new MemoryStream();
        //    await file.CopyToAsync(memoryStream); //возможно memory плохая штука
        //    memoryStream.Position = 0;

        //    var pdfText = new StringBuilder();

        //    using (var pdfDocument = PdfDocument.Open(memoryStream))
        //    {
        //        foreach (var page in pdfDocument.GetPages())
        //        {
        //            pdfText.Append(page.Text);
        //        }
        //    }
        //    return pdfText.ToString();
        //}
    }
}