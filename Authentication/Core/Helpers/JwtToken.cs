using Authentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.Core.Helpers;

public static class JwtToken
{
    public static async Task<string> GenerateJwtToken(User user, AppSettings appSettings)
    {
        //Generate token that is valid for 8 hours
        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken? token = await Task.Run(() =>
        {
            byte[] key = Encoding.ASCII.GetBytes(appSettings.Secret);
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity([new Claim("id", user.UserId.ToString())]),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.CreateToken(tokenDescriptor);
        });

        return tokenHandler.WriteToken(token);
    }
}
