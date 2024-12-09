namespace ChessManager.Domain.Exceptions;

public class InvalidPasswordException : Exception
{
    
    public InvalidPasswordException(string message) : base(message)
    {
    }
    
    
    public InvalidPasswordException()
    {
    }
}