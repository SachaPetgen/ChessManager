using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Exceptions;
using ChessManager.Domain.Models;

namespace ChessManager.Applications.Services;

public class MatchService : IMatchService
{
    private readonly IMatchRepository _matchRepository;
    
    public MatchService(IMatchRepository matchRepository)
    {
        _matchRepository = matchRepository;
    }
    
    public async Task<bool> UpdateResult(int matchId, Result result)
    {
        if (!await _matchRepository.IsMatchCurrentRound(matchId))
        {
            throw new MatchIsNotCurrentRoundException();
        }
        
        return await _matchRepository.UpdateResult(matchId, result);
    }
}