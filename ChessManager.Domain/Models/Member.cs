using ChessManager.Domain.Base;

namespace ChessManager.Domain.Models;

public enum Gender{ M, F, X }
public enum Role{ Admin, Member }

public class Member : BaseEntity
{
    
    public string Pseudo { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public Gender Sex { get; set; }

    public Role Role { get; set; }

    public DateTime BirthDate { get; set; }
}