namespace ChessManager.Infrastructure.Mail.Config;

public class MailConfig
{

    public string NoReplyName { get; }

    public string NoReplyEmail { get; }

    public string SmtpHost { get; }

    public int SmtpPort { get; }

    public MailConfig(string noReplyName, string noReplyEmail, string smtpHost, int smtpPort)
    {
        this.NoReplyName = noReplyName;
        this.NoReplyEmail = noReplyEmail;
        this.SmtpHost = smtpHost;
        this.SmtpPort = smtpPort;
    }

}