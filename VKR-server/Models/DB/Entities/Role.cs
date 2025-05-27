using System.ComponentModel.DataAnnotations.Schema;

namespace VKR_server.Models.DB.Entities;

[Table("roles")]
public class Role
{
    [Column("role_name")] public string RoleName { get; set; }

    public int RoleId { get; set; }

    //[ValidateNever]
    private ICollection<User> Users { get; set; } = new List<User>();
}