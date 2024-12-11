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

    public Task<Match?> CreateMatchAsync(Match entity, int tournamentId, int blackMemberId, int whitePlayerId)
    {
        return _sqlConnection.QuerySingleOrDefaultAsync<Match>(
            "CreateMatch",
            new {entity.Result, },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }


}