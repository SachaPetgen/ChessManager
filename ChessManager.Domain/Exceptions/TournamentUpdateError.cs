namespace ChessManager.Domain.Exceptions;

public class TournamentUpdateError : Exception
{
    
    
    public TournamentUpdateError() : base()
    {
        
    }
    
    public TournamentUpdateError(string message) : base(message)
    {
        
    }
    
}