namespace ChessManager.Domain.Exceptions;

public class InvalidIdentifierException : Exception
{
    
    
    public InvalidIdentifierException(string message) : base(message)
    {
    }
    
    
    public InvalidIdentifierException()
    {
    }
    
}