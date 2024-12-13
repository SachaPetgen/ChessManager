using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ChessManager.Infrastructure.Repository;

public class MatchRepository : IMatchRepository
{

    private readonly SqlConnection _sqlConnection;
    
    public MatchRepository(SqlConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }

    public async Task<Match?> CreateMatchAsync(Result result, int tournamentId, int? blackMemberId, int? whitePlayerId)
    {
        return await _sqlConnection.QuerySingleOrDefaultAsync<Match>(
            "CreateMatch",
            new { TournamentId = tournamentId, WhiteMemberId = whitePlayerId, BlackMemberId = blackMemberId, Result = result },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<bool> AddMatchToTournamentAsync(int tournamentId, int matchId, int round)
    {
        return await _sqlConnection.ExecuteAsync(
            "AddMatchToTournament",
            new { tournamentId, matchId , round},
            commandType: System.Data.CommandType.StoredProcedure) > 0;
    }
    
    public async Task<bool> UpdateResult( int matchId, Result result)
    {
        return await _sqlConnection.ExecuteAsync(
            "UpdateResultMatch",
            new { MatchId = matchId, Result = result },
            commandType: System.Data.CommandType.StoredProcedure) > 0;
    }

    public Task<bool> IsMatchCurrentRound(int matchId)
    {
        return _sqlConnection. QuerySingleOrDefaultAsync<bool>(
            "IsMatchCurrentRound",
            new { MatchId = matchId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}