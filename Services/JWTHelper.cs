using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DotNetEnv;
using Microsoft.IdentityModel.Tokens;
using MyESGIApi.Models;

namespace MyESGIApi.Services
{
    public class JWTHelper
    {
        private static string? SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET");
        private static string? Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
        private static string? Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        public static string GenerateJWT(User user)
        {
            if (SecretKey == null) throw new Exception("JWT_SECRET environment variable not set");
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.EmailAdress),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim("UserId", user.Id.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            if (Encoding.UTF8.GetBytes(SecretKey).Length < 16) throw new Exception("Secret key too short");
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
