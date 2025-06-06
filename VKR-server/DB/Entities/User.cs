﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
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

        [Column("patronymic")]
        [MaxLength(50)]
        public string? Patronymic { get; set; } = string.Empty;

        [Column("email")]
        [Required]
        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Column("password_hash")]
        [Required]
        [MinLength(8)]
        public string Password { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public int? GroupId { get; set; }

        public IEnumerable<File> Files { get; set; } = new List<File>();

        public Role Role { get; set; } = null!;//навигационное свойство

        public Group? Group { get; set; }//навигационное свойство

        [Column("created_at")]
        [Required]
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

    }
}
