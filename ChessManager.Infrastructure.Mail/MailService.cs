using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ChessManager.Infrastructure.Mail;

public class MailService : IMailService
{

    private readonly string _noReplyName;
    private readonly string _noReplyEmail;
    private readonly string _smtpHost;
    private readonly int _smtpPort;
    
    public MailService(string noReplyName, string noReplyEmail, string smtpHost, int smtpPort)
    {
        _noReplyName = noReplyName;
        _noReplyEmail = noReplyEmail;
        _smtpHost = smtpHost;
        _smtpPort = smtpPort;
    }
    
    private SmtpClient GetSmtpClient()
    {
        SmtpClient client = new SmtpClient();
        client.Connect(_smtpHost, _smtpPort);

        return client;
    }
    
    public void SendMail(string emailTo, string name,  string subject, string text)
    {
        
        MimeMessage email = new MimeMessage();

        email.From.Add(new MailboxAddress(_noReplyName, _noReplyEmail));

        email.To.Add(new MailboxAddress(name, emailTo));

        email.Subject = subject;

        email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
        {
            Text = text
        };
        
        using SmtpClient client = GetSmtpClient();

        client.Send(email);
        client.Disconnect(true);
    }
}