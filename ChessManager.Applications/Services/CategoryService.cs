using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Models;

namespace ChessManager.Applications.Services;

public class CategoryService : ICategoryService
{

    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public Task<Category?> CreateAsync(Category entity)
    {
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        return _categoryRepository.CreateAsync(entity);
    }
    
    public Task<Category?> GetByIdAsync(int id)
    {
        return _categoryRepository.GetByIdAsync(id);
    }

}