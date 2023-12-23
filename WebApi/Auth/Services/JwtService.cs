using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Auth.Helpers;
using WebApi.Models;

namespace WebApi.Auth.Services
{
    public class JwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;

        public JwtService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<JwtSecurityToken> CreateToken([Required] ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var key = Encoding.UTF8.GetBytes(_jwt.Key);

            var symmetricSecurityKey = new SymmetricSecurityKey(key);

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public JwtSecurityToken CreateRefreshTokn()
        {
            var key = Encoding.UTF8.GetBytes(_jwt.Key);

            var symmetricSecurityKey = new SymmetricSecurityKey(key);

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
            };

            var jwtRefreshToken = new JwtSecurityToken(
                claims:claims,
                signingCredentials:signingCredentials,
                expires: DateTime.Now.AddDays(_jwt.DurationInMinutes)); // check this , make refresh token lifetime longer

            return jwtRefreshToken;
        }
    }
}

