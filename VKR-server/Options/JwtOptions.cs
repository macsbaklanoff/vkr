using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace VKR_server.Options;

public record JwtOptions
{
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required string Key { get; init; }
    public required int Lifetime { get; init; }

    public bool ValidateIssuer { get; init; } = true;
    public bool ValidateAudience { get; init; } = true;
    public bool ValidateIssuerSigningKey { get; init; } = true;
    public bool ValidateLifetime { get; init; } = true;

    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}