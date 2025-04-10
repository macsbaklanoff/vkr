namespace VKR_server.Dto
{
    public class UserDto : BaseUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
    }
}
