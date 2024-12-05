namespace ChessManager.Domain.Exceptions;

public class BadRegistrationEndDateException : Exception

{

    public BadRegistrationEndDateException() : base()
    {
        
    }
    public BadRegistrationEndDateException(string message) : base(message)
    {
    }
}