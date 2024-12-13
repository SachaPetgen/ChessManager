using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Services;

public interface IMatchService
{
     Task<bool> UpdateResult(int matchId, Result result);
    
}