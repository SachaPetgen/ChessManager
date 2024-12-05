using ChessManager.Infrastructure.Mail.Config;
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
    
    public MailService(MailConfig config)
    {
        _noReplyName = config.NoReplyEmail;
        _noReplyEmail = config.NoReplyEmail;
        _smtpHost = config.SmtpHost;
        _smtpPort = config.SmtpPort;
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