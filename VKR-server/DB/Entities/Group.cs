using System.ComponentModel.DataAnnotations.Schema;

namespace VKR_server.DB.Entities
{
    [Table("groups")]
    public class Group
    {
        public int GroupId { get; set; }

        [Column("group_name")]
        public string GroupName { get; set; }

        ICollection<User> Users { get; set; } = new List<User>();
    }
}
