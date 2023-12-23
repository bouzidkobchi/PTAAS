using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebApi.Auth.Helpers;

namespace WebApi.Auth.Config
{
    public static class JwtTokenValidationParametersFactory
    {
        public static TokenValidationParameters Create(IConfiguration configuration)
        {
            return new TokenValidationParameters
            {
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? throw new Exception("JWT Key is not included in appsettings.json"))),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        }

        public static TokenValidationParameters Create(JWT _jwt)
        {
            return new TokenValidationParameters
            {
                ValidIssuer = _jwt.Issuer,
                ValidAudience = _jwt.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwt.Key ?? throw new Exception("JWT Key is not included in appsettings.json"))),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };
        }
    }


}
