namespace ChessManager.Domain.Exceptions;

public class UnableToRegisterMemberException : Exception
{
    
    public UnableToRegisterMemberException(string message) : base(message)
    {
        
    }
    
    public UnableToRegisterMemberException() : base()
    {
        
    }
    
}