using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using VKR_server.Models.DB;
using VKR_server.Options;

namespace VKR_server.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.ConfigureOptions<AiOptions>();
        services.ConfigureOptions<JwtOptions>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policyBuilder =>
            {
                policyBuilder.WithOrigins("http://localhost:5173", "http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        services.AddDbContext<ApplicationContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();

        services.AddOptions<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme)
            .Configure<IOptions<JwtOptions>>((jwtOptions, authOptions) =>
            {
                var opts = authOptions.Value;

                jwtOptions.RequireHttpsMetadata = false;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = opts.ValidateIssuer,
                    ValidateAudience = opts.ValidateAudience,
                    ValidateIssuerSigningKey = opts.ValidateIssuerSigningKey,
                    ValidateLifetime = opts.ValidateLifetime,
                    ValidIssuer = opts.Issuer,
                    ValidAudience = opts.Audience,
                    IssuerSigningKey = opts.GetSymmetricSecurityKey()
                };
            });

        return services;
    }
}
