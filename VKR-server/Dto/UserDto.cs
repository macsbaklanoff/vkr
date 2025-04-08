namespace VKR_server.Dto
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
