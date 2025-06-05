using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace VKR_server.JWT
{
    public class AuthOptions
    {
        public const string ISSUER = "VKR_server";
        public const string AUDIENCE = "VKR_client";
        public const string key = "key_for_vipusknoy_kvalification_work";
        public const int LIFETIME = 30;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }

    }
}
