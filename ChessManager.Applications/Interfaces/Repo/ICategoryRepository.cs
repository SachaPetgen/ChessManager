using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Repo;

public interface ICategoryRepository : IBaseRepository<Category>
{

     Task<Category?> GetByIdAsync(int id);
     
     Task<IEnumerable<Category>> GetAllAsync();
     
     Task<Category?> CreateAsync(Category entity);

}