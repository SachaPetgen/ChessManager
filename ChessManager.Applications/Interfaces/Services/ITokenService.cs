using ChessManager.Domain.Models;
using System.Security.Claims;

namespace ChessManager.Applications.Interfaces.Services;

public interface ITokenService
{
    string GenerateToken(Member member);

}