using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Together;
using Together.Models.ChatCompletions;
using VKR_server.Models.Dto;
using VKR_server.Options;

namespace VKR_server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController(ILogger<FileController> logger, IOptions<AiOptions> aiOptions) : ControllerBase
{
    private Kernel _kernel;

    [Authorize]
    [HttpPost("upload-file", Name = "UploadFile")]
    public async Task<IActionResult> UploadFile([FromForm] UploadFileDto uploadFileRequest)
    {
        var client = new TogetherClient(aiOptions.Value.TogetherAiApiKey);

        var response = await client.ChatCompletions.CreateAsync(new ChatCompletionRequest
        {
            Model = aiOptions.Value.ModelName,
            Messages =
            [
                new ChatCompletionMessage { Role = ChatRole.User, Content = "Hello!" }
            ]
        });

        return Ok(response);
    }
}
