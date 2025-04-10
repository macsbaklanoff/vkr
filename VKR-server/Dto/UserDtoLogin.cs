
using VKR_server.Interfaces;

namespace VKR_server.Dto
{
    public class UserDtoAuth : IUserAuth
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
