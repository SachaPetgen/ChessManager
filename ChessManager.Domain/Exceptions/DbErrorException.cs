namespace ChessManager.Domain.Exceptions;

public class DbErrorException : Exception
{
    
    public DbErrorException(string message) : base(message)
    {
        
    }
    
}