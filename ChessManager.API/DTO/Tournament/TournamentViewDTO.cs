using ChessManager.Domain.Models;
using ChessManager.DTO.Member;

namespace ChessManager.DTO.Tournament;

public class TournamentViewDTO
{
    public int Id { get; set; }
    
    public string Name { get; set; }

    public string Location { get; set; }

    public int MaxPlayerCount { get; set; }

    public int MinPlayerCount { get; set; }

    public int MaxEloAllowed { get; set; }

    public int MinEloAllowed { get; set; }

    public Status Status { get; set; }

    public int CurrentRound { get; set; }

    public bool WomenOnly { get; set; }

    public DateTime RegistrationEndDate { get; set; }
    
    public IEnumerable<MemberViewListDTO> Members { get; set; }
}