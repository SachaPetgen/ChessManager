namespace ChessManager.Applications.Interfaces.Services;

public interface IPasswordService
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string inputPassword);
    string GenerateRandomPassword();
}