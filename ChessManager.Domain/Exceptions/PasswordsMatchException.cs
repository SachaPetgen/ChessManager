namespace ChessManager.Domain.Exceptions;

public class PasswordsMatchException: Exception
{
    
    public PasswordsMatchException() : base()
    {
        
    }
    
    public PasswordsMatchException(string message) : base(message)
    {
    }
    
}