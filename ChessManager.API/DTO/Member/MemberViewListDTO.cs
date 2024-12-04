using ChessManager.Domain.Models;

namespace ChessManager.DTO.Member;

public  class MemberViewListDTO
{
    
    public int Id { get; set; }
    public string Pseudo { get; set; }

    public string Email { get; set; }
    
    public Gender Gender { get; set; }
    
    public int Elo { get; set; }


    public Role Role { get; set; }

    public DateTime BirthDate { get; set; }
}