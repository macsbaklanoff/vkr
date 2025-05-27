using VKR_server.Models.Interfaces;

namespace VKR_server.Models.Dto;

public class UserDtoAuth : IUserAuth
{
    public string Email { get; set; }
    public string Password { get; set; }
}