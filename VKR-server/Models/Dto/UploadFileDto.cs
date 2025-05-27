namespace VKR_server.Models.Dto;

public class UploadFileDto
{
    public string UserId { get; set; } //строка потому что FormData только строки
    public IFormFile File { get; set; }
}