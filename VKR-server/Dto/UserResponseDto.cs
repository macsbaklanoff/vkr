using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VKR_server.Dto
{
    public class UserResponseDto
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public int? GroupId { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    }
}
