using ChessManager.Domain.Models;

namespace ChessManager.Domain.Exceptions;

public class TournamentUnableToStart : Exception
{


    public TournamentUnableToStart() : base()
    {
        
    }
    
    public TournamentUnableToStart(string message) : base(message)
    {
        
    }
}