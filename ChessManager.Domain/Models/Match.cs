using ChessManager.Domain.Base;

namespace ChessManager.Domain.Models;


public enum Result
{
    PENDING,
    WHITEWINS,
    BLACKWINS,
    DRAW
}
public class Match : BaseEntity
{
    public Result Result { get; set; }
    
    public int Round { get; set; }
    
}