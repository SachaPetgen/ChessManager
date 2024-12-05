using System.ComponentModel.DataAnnotations;
using ChessManager.Domain.Models;

namespace ChessManager.DTO.Member;

public class MemberCreateDTO
{
    [Required]
    public string Pseudo { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public Gender Gender { get; set; }
    
    [Required]
    public Role Role { get; set; }

    public int? Elo { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }
}