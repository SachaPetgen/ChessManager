using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Services;

public interface ITournamentService
{
    Task<Tournament?> GetByIdAsync(int id);

    Task<IEnumerable<Tournament>> GetAllAsync();
    
    Task<IEnumerable<Tournament>> GetLastModified(int number);

    Task<Tournament?> CreateAsync(Tournament entity);
    
    Task<bool> DeleteAsync(int id);
    
    Task<bool> RegisterMember(int memberId, int tournamentId);
    
    Task<bool> UnregisterMember(int memberId, int tournamentId);
    
    Task<bool> AddCategory(int tournamentId, int categoryId);
    
    Task<bool> StartTournament(int tournamentId);
    
    
}