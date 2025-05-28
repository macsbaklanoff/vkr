using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using VKR_server.DB;
using VKR_server.Dto;
using VKR_server.JWT;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.AI;
using Together.Models.ChatCompletions;
using Together;
using Together.SemanticKernel.Extensions;
using System.Text;
using UglyToad.PdfPig;
using System.Text.RegularExpressions;

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
        //meta-llama/Llama-Vision-Free
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
            var textContent = ReadPdfFile(uploadFile.File);
            //Console.WriteLine(textContent.Result);
            var chatResult = await _kernel.InvokePromptAsync($"Оцени содержимое файла по содержанию и " +
                $"глубине проработке темы от 0 до 100 баллов" +
                $"Обоснуй оценку и " +
                $"Ответ дай **строго** в следующем формате: " +
                $"Score: число" +
                $"Текст для оценки:" +
                $" {textContent.Result}");
            var match = Regex.Match(chatResult.GetValue<string>(), @"Score:\s(\d+)");
            int score = 0;
            if (match.Success)
            {
                score = int.Parse(match.Groups[1].Value);
            }
            Console.WriteLine(score);
            return Ok();
        }
        private async Task<string> ReadPdfFile(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream); //возможно memory плохая штука
            memoryStream.Position = 0;

            var pdfText = new StringBuilder();

            using (var pdfDocument = PdfDocument.Open(memoryStream))
            {
                foreach (var page in pdfDocument.GetPages())
                {
                    pdfText.Append(page.Text);
                }
            }
            return pdfText.ToString();
        }
    }
}