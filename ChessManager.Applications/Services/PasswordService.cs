using System.Runtime.InteropServices.JavaScript;
using ChessManager.Applications.Interfaces.Services;
using Isopoh.Cryptography.Argon2;

namespace ChessManager.Applications.Services;

public class PasswordService : IPasswordService
{

    public string HashPassword(string password)
    {
        return Argon2.Hash(password);

    }

    public bool VerifyPassword(string hashedPassword, string inputPassword)
    {
        return Argon2.Verify(inputPassword, hashedPassword);
    }

    public string GenerateRandomPassword()
    {
        return new string(Guid.NewGuid().ToByteArray().Select(t => Convert.ToChar((t % 50) + 65)).ToArray());
    }

}