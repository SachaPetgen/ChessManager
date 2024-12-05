using System.Data;
using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ChessManager.Infrastructure.Repository;

public class MemberRepository : IMemberRepository
{
    
    private readonly SqlConnection _sqlConnection;
    
    public MemberRepository(SqlConnection sqlConnection)
    {
       _sqlConnection = sqlConnection;
    }

    public Task<Member?> GetByIdAsync(int id)
    {
        return _sqlConnection.QuerySingleOrDefaultAsync<Member?>(
            "GetMemberById",
            new { MemberId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public Task<Member?> GetByEmail(string email)
    {
        return _sqlConnection.QuerySingleOrDefaultAsync<Member?>(
            "GetMemberByEmail",
            new { Email = email },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public Task<IEnumerable<Member>> GetAllAsync()
    {
        return _sqlConnection.QueryAsync<Member>(
            "GetAllMembers",
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public async Task<Member?> CreateAsync(Member entity)
    {
        return await _sqlConnection.QuerySingleOrDefaultAsync<Member>(
            "CreateMember",
            new { entity.Pseudo, entity.Email, entity.Password, entity.Role, entity.Gender, entity.Elo, entity.BirthDate, entity.CreatedAt, entity.UpdatedAt },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

}