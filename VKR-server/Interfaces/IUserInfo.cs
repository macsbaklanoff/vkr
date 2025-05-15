using System.Runtime.InteropServices;

namespace VKR_server.Interfaces
{
    public interface IUserInfo
    {
        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId {  get; set; }

        public string? GroupName { get; set; }
        
    }
}
