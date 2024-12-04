namespace ChessManager.Infrastructure.Mail;

public interface IMailService
{


    public void SendMail(string emailTo, string name, string subject, string text);
}