namespace ChessManager.Domain.Exceptions;

public class MatchIsNotCurrentRoundException : Exception
{
    
    
    public MatchIsNotCurrentRoundException() : base()
    {
        
    }
    
    public MatchIsNotCurrentRoundException(string message) : base(message)
    {
        
    }
}