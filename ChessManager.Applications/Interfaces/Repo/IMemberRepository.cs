using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Repo;

public interface IMemberRepository : IBaseRepository<Member>
{
    
    
    Task<Member?> GetByEmail(string email);
    
    

}