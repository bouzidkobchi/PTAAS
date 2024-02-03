using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Auth.Config;
using WebApi.Auth.Helpers;
using WebApi.Models;

namespace WebApi.Auth.Services
{
    public class JwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
        private readonly JWT _jwt;

        public JwtService(UserManager<ApplicationUser> userManager,
            IOptions<JWT> jwt,
            JwtSecurityTokenHandler jwtSecurityTokenHandler)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _jwtSecurityTokenHandler = jwtSecurityTokenHandler;
        }

        public async Task<JwtSecurityToken> CreateAccessToken([Required] ApplicationUser user)
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
                expires: DateTime.Now.AddDays(_jwt.AccessTokenDuration),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public JwtSecurityToken CreateRefreshToken([Required] ApplicationUser user)
        {
            var key = Encoding.UTF8.GetBytes(_jwt.Key);

            var symmetricSecurityKey = new SymmetricSecurityKey(key);

            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                new Claim("uid", user.Id)
            };

            var jwtRefreshToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.Now.AddDays(_jwt.RefreshTokenDuration));

            return jwtRefreshToken;
        }

        //public TokenResponse RefreshAccessToken(string refreshToken)
        //{
        //    // Validate and decode the refresh token
        //    var refreshTokenHandler = new JwtSecurityTokenHandler();
        //    var validationParameters = new TokenValidationParameters
        //    {
        //        ValidateIssuerSigningKey = true,
        //        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_jwt.Key)),
        //        ValidateIssuer = false,
        //        ValidateAudience = false,
        //        ValidateLifetime = false // Allows expired tokens for refresh
        //    };

        //    var principal = refreshTokenHandler.ValidateToken(refreshToken, validationParameters, out var securityToken);

        //    // You might have additional logic here, such as checking if the token is associated with a specific user

        //    // Generate a new access token and refresh token
        //    var newAccessToken = /* Generate new access token */ "access" ;
        //    var newRefreshToken = /* Generate new refresh token */ "refresh" ;

        //    // Return the new tokens
        //    return new TokenResponse
        //    {
        //        AccessToken = newAccessToken,
        //        RefreshToken = newRefreshToken,
        //        ExpiresOn = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_jwt.AccessTokenDuration))
        //    };
        //}

        public ClaimsPrincipal? ReadToken(string token, out JwtSecurityToken? refreshTokenObject)
        {
            try
            {
                var tokenValidationParameters = JwtTokenValidationParametersFactory.Create(_jwt);

                var User = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    refreshTokenObject = null;
                    return null;
                }

                refreshTokenObject = (JwtSecurityToken)securityToken;
                return User;
            }
            catch (Exception)
            {
                // Handle other exceptions if necessary
                refreshTokenObject = null;
                return null;
            }
        }

    }

    //public class TokenResponse
    //{
    //    public string AccessToken { get; set; }
    //    public string RefreshToken { get; set; }
    //    public DateTime ExpiresOn { get; set; }
    //}

}

