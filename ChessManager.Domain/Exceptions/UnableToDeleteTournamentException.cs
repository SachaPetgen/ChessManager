namespace ChessManager.Domain.Exceptions;

public class UnableToDeleteTournamentException : Exception
{
    
    public UnableToDeleteTournamentException() : base()
    {
        
    }
    public UnableToDeleteTournamentException(string message) : base(message)
    {
    }
    
}