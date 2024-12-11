using _1likteEcommerce.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace _1likteEcommerce.Business.Services
{
    public class UserService : IUserService
    {
        public string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = System.Text.Encoding.UTF8.GetBytes("81beca0d490431b65b22bfa2d86a2495d2492235daee928cb8d784ce751b42a8");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
