namespace VKR_server.Dto
{
    public class UploadFileDto
    {
        public int UserId { get; set; }
        public IFormFile File { get; set; }
    }
}
