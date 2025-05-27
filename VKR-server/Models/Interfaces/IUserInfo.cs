namespace VKR_server.Models.Interfaces;

public interface IUserInfo
{
    public int? UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int RoleId { get; set; }

    public string? GroupName { get; set; }
}