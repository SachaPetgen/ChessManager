namespace ChessManager.Domain.Exceptions;

public class TournamentNotFinishedException : Exception
{
    
    
    public TournamentNotFinishedException() : base()
    {
        
    }
    
    public TournamentNotFinishedException(string message) : base(message)
    {
        
    }
}