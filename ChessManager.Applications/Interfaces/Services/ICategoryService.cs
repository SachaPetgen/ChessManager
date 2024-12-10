using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Services;

public interface ICategoryService 
{
    
    
    public Task<Category?> CreateAsync(Category entity);
    
    public Task<Category?> GetByIdAsync(int id);
}