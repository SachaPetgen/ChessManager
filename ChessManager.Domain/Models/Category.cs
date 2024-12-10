using ChessManager.Domain.Base;

namespace ChessManager.Domain.Models;

public class Category : BaseEntity
{
    
    
    public string Name { get; set; }
    
    public int AgeMax { get; set; }
    
    public int AgeMin { get; set; }
    
    
    
}