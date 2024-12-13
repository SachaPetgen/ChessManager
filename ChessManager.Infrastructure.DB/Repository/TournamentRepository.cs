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
    
    public async Task<bool> CheckIfMemberIsRegistered(int memberId, int tournamentId)
    {
        return await _sqlConnection.QuerySingleOrDefaultAsync<bool>(
            "IsPlayerRegisteredTournament",
            new { MemberId = memberId, TournamentId = tournamentId },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public async Task<IEnumerable<Category>> GetCategories(int id)
    {
        return await _sqlConnection.QueryAsync<Category>(
            "GetTournamentCategories",
            new { TournamentId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public async Task<int> GetNumberOfRegisteredMembers(int id)
    {
        return await _sqlConnection.QuerySingleOrDefaultAsync<int>(
            "GetTournamentMembersCount",
            new { TournamentId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public async Task<bool> RegisterMember(int memberId, int tournamentId)
    {
        return await _sqlConnection.ExecuteAsync(
            "RegisterMemberToTournament",
            new { MemberId = memberId, TournamentId = tournamentId },
            commandType: System.Data.CommandType.StoredProcedure
        ) > 0;
    }
    
    public async Task<bool> UnregisterMember(int memberId, int tournamentId)
    {
        return await _sqlConnection.ExecuteAsync(
            "UnregisterMemberFromTournament",
            new { MemberId = memberId, TournamentId = tournamentId },
            commandType: System.Data.CommandType.StoredProcedure
        ) > 0;
    }
    
    public async Task<bool> AddCategory(int tournamentId, int categoryId)
    {
        return await _sqlConnection.ExecuteAsync(
            "AddCategoryToTournament",
            new { TournamentId = tournamentId, CategoryId = categoryId },
            commandType: System.Data.CommandType.StoredProcedure
        ) > 0;
    }
    
    public async Task<bool> StartTournament(int tournamentId)
    {
        return await _sqlConnection.ExecuteAsync(
            "StartTournament",
            new { TournamentId = tournamentId, Status = 1 , CurrentRound = 1  },
            commandType: System.Data.CommandType.StoredProcedure
        ) > 0;
    }

    public async Task<bool> StartNextRound(int tournamentId)
    {
        var x = await _sqlConnection.QueryFirstOrDefaultAsync<int>(
            "StartNextRoundTournament",
            new { TournamentId = tournamentId },
            commandType: System.Data.CommandType.StoredProcedure);
        return x > 0;
    }
    
    public async Task<IEnumerable<Match>> GetMatches(int id)
    {
        return await _sqlConnection.QueryAsync<Match>(
            "GetAllMatchesOfTournament",
            new { TournamentId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
}