using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DashyBoard.Application.Common.Interfaces;
using DashyBoard.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DashyBoard.Infrastructure.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;

    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(Admin admin)
    {
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!)
        );

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, admin.FirstName ?? ""),
            new Claim(JwtRegisteredClaimNames.FamilyName, admin.LastName ?? ""),
            new Claim(ClaimTypes.Role, admin.Role ?? ""),
            new Claim("HotelId", admin.HotelId?.ToString() ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(double.Parse(_configuration["Jwt:ExpirationHours"]!)),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
