using ClinicaVeterinaria.API.Api.model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ClinicaVeterinaria.API.Api.services.tokens
{
    public static class TokenService
    {
        public static string? CreateToken(IUser user, IConfiguration? config)
        {
            if (config == null) return null;
            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("id", user.Id.ToString()),
                        new Claim("username", user.Name.ToString()),
                        new Claim("surname", user.Surname.ToString()),
                        new Claim("email", user.Email),
                        new Claim("role", Roles.ToString(user.Role))
                    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                config["Jwt:Issuer"],
                config["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
