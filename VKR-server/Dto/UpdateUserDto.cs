namespace VKR_server.Dto
{
    public class UpdateUserDto
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronymic { get; set; }

        public string? Email { get; set; }

        public string? GroupName { get; set; }
    }
}
