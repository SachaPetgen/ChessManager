using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Repo;

public interface ITournamentRepository : IBaseRepository<Tournament>
{
    
    Task<IEnumerable<Tournament>> GetLastModified(int number);

    Task<IEnumerable<Member>> GetMembers(int id);
    
    Task<bool> DeleteAsync(int id);
    
    Task<bool> CheckIfMemberIsRegistered(int memberId, int tournamentId);
    
    Task<IEnumerable<Category>> GetCategories(int id);
    
    Task<int> GetNumberOfRegisteredMembers(int id);
    
    Task<bool> RegisterMember(int memberId, int tournamentId);
    
    Task<bool> UnregisterMember(int memberId, int tournamentId);
    
    Task<bool> AddCategory(int tournamentId, int categoryId);
    Task<bool> StartTournament(int tournamentId);
}