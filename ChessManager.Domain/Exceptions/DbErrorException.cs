namespace ChessManager.Domain.Exceptions;

public class DbErrorException : Exception
{

    public DbErrorException() : base()
    {
        
    }
    public DbErrorException(string message) : base(message)
    {
        
    }
    
}