using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebApi.Auth.Helpers;
using WebApi.Models;

namespace WebApi.Auth.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JWT _jwt;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            ArgumentNullException.ThrowIfNull(user);

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
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}

//        public async Task<AuthModel> RegisterAsync(RegisterModel model)
//        {
//            if (await _userManager.FindByEmailAsync(model.Email) is not null)
//                return new AuthModel 
//                {
//                    Message = "Email is already registered!",
//                    statusCode = HttpStatusCode.BadRequest
//                };

//            if (await _userManager.FindByNameAsync(model.UserName) is not null)
//                return new AuthModel 
//                { 
//                    Message = "Username is already registered!",
//                    statusCode = HttpStatusCode.BadRequest 
//                };

//            var user = new Client
//            {
//                UserName = model.UserName,
//                Email = model.Email,
//            };

//            var result = await _userManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                var errors = new List<string>();

//                foreach (var error in result.Errors)
//                    errors.Add(error.Description);

//                return new AuthModel 
//                {
//                    Message = "Failed to register this user!",
//                    Errors = errors,
//                    statusCode = HttpStatusCode.BadRequest
//                };
//            }

//            // default user role
//            await _userManager.AddToRoleAsync(user, "User");

//            var jwtSecurityToken = await CreateJwtToken(user);

//            return new AuthModel
//            {
//                Email = user.Email,
//                ExpiresOn = jwtSecurityToken.ValidTo,
//                IsAuthenticated = true,
//                Roles = new List<string> { "User" },
//                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
//                Username = user.UserName,
//                statusCode = HttpStatusCode.OK
//            };
//        }

//        public async Task<AuthModel> GetTokenAsync(LoginModel model)
//        {
//            var authModel = new AuthModel();

//            var user = await _userManager.FindByNameAsync(model.UserName);

//            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
//            {
//                authModel.Message = "UserName or Password is incorrect!";
//                authModel.statusCode = HttpStatusCode.BadRequest;
//                return authModel;
//            }

//            var jwtSecurityToken = await CreateJwtToken(user);
//            var rolesList = await _userManager.GetRolesAsync(user);

//            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
//            authModel.Email = user.Email;
//            authModel.Username = user.UserName;
//            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
//            authModel.Roles = rolesList.ToList();
//            authModel.statusCode = HttpStatusCode.OK

//            return authModel;
//        }

//        public async Task<AuthModel> AddRoleAsync(AddRoleModel model)
//        {
//            var user = await _userManager.FindByIdAsync(model.UserId);

//            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
//                return new ;

//            if (await _userManager.IsInRoleAsync(user, model.Role))
//                return "User already assigned to this role";

//            var result = await _userManager.AddToRoleAsync(user, model.Role);

//            return result.Succeeded ? string.Empty : "Sonething went wrong";
//        }

//        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
//        {
//            var userClaims = await _userManager.GetClaimsAsync(user);
//            var roles = await _userManager.GetRolesAsync(user);
//            var roleClaims = new List<Claim>();

//            foreach (var role in roles)
//                roleClaims.Add(new Claim("roles", role));

//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(JwtRegisteredClaimNames.Email, user.Email),
//                new Claim("uid", user.Id)
//            }
//            .Union(userClaims)
//            .Union(roleClaims);

//            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
//            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

//            var jwtSecurityToken = new JwtSecurityToken(
//                issuer: _jwt.Issuer,
//                audience: _jwt.Audience,
//                claims: claims,
//                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
//                signingCredentials: signingCredentials);

//            return jwtSecurityToken;
//        }
//    }
//}
