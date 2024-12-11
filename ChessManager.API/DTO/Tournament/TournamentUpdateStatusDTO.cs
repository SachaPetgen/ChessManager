using ChessManager.Domain.Models;

namespace ChessManager.DTO.Tournament;

public class TournamentUpdateStatusDTO
{
    public int tournamentId { get; set; }
    
    public Status Status { get; set; }
}