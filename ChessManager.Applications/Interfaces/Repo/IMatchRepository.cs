
using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Repo;

public interface IMatchRepository
{


    Task<Match?> CreateMatchAsync(Result result, int tournamentId, int? blackMemberId, int? whitePlayerId);
    
    Task<bool> AddMatchToTournamentAsync(int tournamentId, int matchId, int round);

    Task<bool> UpdateResult(int matchId, Result result);
    
    Task<bool> IsMatchCurrentRound(int matchId);


}