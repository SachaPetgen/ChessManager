using ChessManager.Domain.Base;

namespace ChessManager.Domain.Models;

public enum Status
{
    PENDING,
    ONGOING,
    OVER
}

public class Tournament : BaseEntity
{
    
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

    public IEnumerable<Member>? Members { get; set; }

}