namespace ChessManager.Domain.Exceptions;

public class MemberNotRegisteredException : Exception
{
    
    public MemberNotRegisteredException() : base()
    {
        
    }
    
    public MemberNotRegisteredException(string message) : base(message)
    {
    }
    
}