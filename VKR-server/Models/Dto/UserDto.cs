using VKR_server.Models.Interfaces;

namespace VKR_server.Models.Dto;

public class UserDto : IUser
{
    public string? RoleName { get; set; }
    public int? UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int RoleId { get; set; }

    public string? GroupName { get; set; }
}