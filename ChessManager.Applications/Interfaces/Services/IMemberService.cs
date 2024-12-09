using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Services;


public interface IMemberService
{

    public Task<Member?> GetByIdAsync(int id);

    public Task<IEnumerable<Member>> GetAllAsync();

    public Task<Member?> CreateAsync(Member entity);
    
    public Task<string> Login(string identifier, string password);
}