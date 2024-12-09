using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Services;

public interface ITokenService
{


    public string GenerateToken(Member member);

}