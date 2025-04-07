using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VKR_server.DB.Entities
{
    [Table("users")]
    public class User
    {
        [Column("user_id")]
        public int Id { get; set; }

        [Column("first_name")]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Column("last_name")]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Column("password_hash")]
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;//навигационное свойство

        [Column("created_at")]
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    }
}
