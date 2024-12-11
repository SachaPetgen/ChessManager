using System.ComponentModel.DataAnnotations;
using ChessManager.Domain.Models;

namespace ChessManager.DTO.Tournament;

public class TournamentCreateDTO
{
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Location { get; set; }
    
    [Required]
    public int MaxPlayerCount { get; set; }
    
    [Required]
    public int MinPlayerCount { get; set; }
    
    [Required]
    public int MaxEloAllowed { get; set; }
    
    [Required]
    public int MinEloAllowed { get; set; }
    
    [Required]
    public bool WomenOnly { get; set; }
    
    [Required]
    public DateTime RegistrationEndDate { get; set; }
    
}