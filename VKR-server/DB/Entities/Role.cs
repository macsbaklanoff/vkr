using System.ComponentModel.DataAnnotations.Schema;

namespace VKR_server.DB.Entities
{
    [Table("roles")]
    public class Role
    {
        [Column("role_name")]
        public string RoleName { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }

        //[ValidateNever]
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
