namespace VKR_server.Dto
{
    public class UploadFileDto
    {
        public string UserId { get; set; } //строка потому что FormData только строки
        public string TopicWork {  get; set; }
        public string AcademicSubject { get; set; }
        public IFormFile File { get; init; }
    }
}
