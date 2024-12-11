
using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Repo;

public interface IMatchRepository
{


    Task<Match?> CreateMatchAsync(Match entity, int tournamentId, int blackMemberId, int whitePlayerId);
    
    
}