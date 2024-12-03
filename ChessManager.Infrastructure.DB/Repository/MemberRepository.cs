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

    public async Task<Member?> GetByIdAsync(int id)
    {
        return await _sqlConnection.QuerySingleOrDefaultAsync<Member?>(
            "GetMemberById",
            new { id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }

    public Task<IEnumerable<Member>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Member> CreateAsync(Member entity)
    {
        throw new NotImplementedException();
    }

    public Task<Member> UpdateAsync(Member entity)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

}