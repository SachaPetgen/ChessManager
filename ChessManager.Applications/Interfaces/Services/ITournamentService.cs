using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Services;

public interface ITournamentService
{
    public Task<Tournament?> GetByIdAsync(int id);

    public Task<IEnumerable<Tournament>> GetAllAsync();
    
    public Task<IEnumerable<Tournament>> GetLastModified(int number);

    public Task<Tournament?> CreateAsync(Tournament entity);
    
    public Task<bool> DeleteAsync(int id);
    
    
}