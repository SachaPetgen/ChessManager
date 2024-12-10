using ChessManager.Domain.Models;

namespace ChessManager.Applications.Interfaces.Services;


public interface IMemberService
{

    Task<Member?> GetByIdAsync(int id);

    Task<IEnumerable<Member>> GetAllAsync();

    Task<Member?> CreateAsync(Member entity);
    
    Task<string> Login(string identifier, string password);
    
    Task<bool> ChangePassword(int id, string newPassword, string newPasswordConfirmation, string oldPassword);
}