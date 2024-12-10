using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace ChessManager.Services;

public class TokenService : ITokenService
{
    
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
        
    public string GenerateToken(Member member)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, member.Id.ToString()),
            new Claim("Username", member.Pseudo),
            new Claim("Elo", member.Elo.ToString()!),
            new Claim(ClaimTypes.Email, member.Email),
            new Claim(ClaimTypes.Role, member.Role.ToString()),
            new Claim(JwtRegisteredClaimNames.Exp, DateTime.UtcNow.AddMinutes(15).ToString())  // Optional but adds clarity
        };
    
        // Key for signing the token
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

        // Signing credentials with the encryption key and algorithm
        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        // Generate the token
        JwtSecurityToken token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"], // Issuer of the token (usually your API server)
            _configuration["Jwt:Audience"], // Audience that uses the token (client side)
            claims,
            expires: DateTime.UtcNow.AddMinutes(15), // Token expiration (short-lived access token)
            signingCredentials: creds
        );

        // Return the token as a string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}