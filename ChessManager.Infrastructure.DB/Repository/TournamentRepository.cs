using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ChessManager.Infrastructure.Repository;

public class TournamentRepository : ITournamentRepository
{

    private readonly SqlConnection _sqlConnection;
    
    public TournamentRepository(SqlConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }
    
    public Task<Tournament?> GetByIdAsync(int id)
    {
        return _sqlConnection.QuerySingleOrDefaultAsync<Tournament?>(
            "GetTournamentById",
            new { TournamentId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public Task<IEnumerable<Tournament>> GetAllAsync()
    {
        return _sqlConnection.QueryAsync<Tournament>(
            "GetAllTournaments",
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public Task<IEnumerable<Tournament>> GetLastModified(int number)
    {
        return _sqlConnection.QueryAsync<Tournament>(
            "GetLastModifiedTournaments",
            new { Number = number },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public Task<IEnumerable<Member>> GetMembers(int id)
    {
        return _sqlConnection.QueryAsync<Member>(
            "GetTournamentMembers",
            new { TournamentId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public async Task<Tournament?> CreateAsync(Tournament entity)
    {
        return await _sqlConnection.QuerySingleOrDefaultAsync<Tournament>(
            "CreateTournament",
            new { entity.Name, entity.Location, entity.MaxPlayerCount, entity.MinPlayerCount, entity.MaxEloAllowed, entity.MinEloAllowed, entity.Status, entity.CurrentRound, entity.WomenOnly, entity.RegistrationEndDate, entity.CreatedAt, entity.UpdatedAt },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<bool> DeleteAsync(int id)
    {
        int affectedRows = await _sqlConnection.ExecuteAsync(
            "DeleteTournament",
            new { TournamentId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
        
        return affectedRows > 0;
    }
}