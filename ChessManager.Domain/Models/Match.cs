using ChessManager.Domain.Base;

namespace ChessManager.Domain.Models;


public enum Result
{
    ODDGAME,
    PENDING,
    WHITEWINS,
    BLACKWINS,
    DRAW
}
public class Match : BaseEntity
{
    public Result Result { get; set; }
    
    public int WhitePLayerId { get; set; }
    
    public int BlackPlayerId { get; set; }
    
}