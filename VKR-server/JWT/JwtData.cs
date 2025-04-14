using VKR_server.Interfaces;

namespace VKR_server.JWT
{
    public class JwtData : IJWTData
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string RoleName { get; set; }
    }
}
