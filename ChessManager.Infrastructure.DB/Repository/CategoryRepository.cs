using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ChessManager.Infrastructure.Repository;

public class CategoryRepository : ICategoryRepository
{
        
    private readonly SqlConnection _sqlConnection;
    
    public CategoryRepository(SqlConnection sqlConnection)
    {
        _sqlConnection = sqlConnection;
    }
    
    public Task<Category?> GetByIdAsync(int id)
    {
        return _sqlConnection.QuerySingleOrDefaultAsync<Category?>(
            "GetCategoryById",
            new { CategoryId = id },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public Task<IEnumerable<Category>> GetAllAsync()
    {
        return _sqlConnection.QueryAsync<Category>(
            "GetAllCategories",
            commandType: System.Data.CommandType.StoredProcedure
        );
    }
    
    public Task<Category?> CreateAsync(Category entity)
    {
        return _sqlConnection.QuerySingleOrDefaultAsync<Category>(
            "CreateCategory",
            new { entity.Name, entity.AgeMax, entity.AgeMin, entity.CreatedAt, entity.UpdatedAt },
            commandType: System.Data.CommandType.StoredProcedure
        );
    }


}