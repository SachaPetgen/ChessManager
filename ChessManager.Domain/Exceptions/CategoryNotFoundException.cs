namespace ChessManager.Domain.Exceptions;

public class CategoryNotFoundException : Exception
{

    public CategoryNotFoundException() : base()
    {
        
    }
    
    public CategoryNotFoundException(string message) : base(message)
    {
    }
    
    
    
    
}