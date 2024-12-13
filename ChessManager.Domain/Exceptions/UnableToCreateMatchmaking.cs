namespace ChessManager.Domain.Exceptions;

public class UnableToCreateMatchmaking : Exception
{


    public UnableToCreateMatchmaking() : base()
    {
        
    }
    
    public UnableToCreateMatchmaking(string message) : base(message)
    {
        
    }
    
    
}