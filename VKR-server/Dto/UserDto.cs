﻿using VKR_server.Interfaces;

namespace VKR_server.Dto
{
    public class UserDto : IUser
    {
        public int? UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }

        public string? GroupName { get; set; }
        public string? RoleName { get; set; }
    }
}
