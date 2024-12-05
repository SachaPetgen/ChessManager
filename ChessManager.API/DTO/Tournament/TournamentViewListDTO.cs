using ChessManager.Domain.Models;

namespace ChessManager.DTO.Tournament;

public class TournamentViewListDTO
{
    
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string Location { get; set; }

    public int MaxPlayerCount { get; set; }
    
    public int MaxEloAllowed { get; set; }
    
    public Status Status { get; set; }
}