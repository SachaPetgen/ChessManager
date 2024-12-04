using ChessManager.Domain.Base;

namespace ChessManager.Applications.Interfaces.Services;

public interface IBaseService<T> where T : BaseEntity
{
    
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> CreateAsync(T entity);
    
}