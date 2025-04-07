using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace VKR_server.DB.Entities
{
    [Table("roles")]
    public class Role
    {
        [Key]
        [Column("role_name")]
        public string RoleName { get; set; }

        [ValidateNever]
        List<User> Users { get; set; }
    }
}
