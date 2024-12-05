using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Repo;

public interface ITournamentRepository : IBaseRepository<Tournament>
{
    
    Task<IEnumerable<Tournament>> GetLastModified(int number);

    Task<IEnumerable<Member>> GetMembers(int id);
    
    Task<bool> DeleteAsync(int id);
}