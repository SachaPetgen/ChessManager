using ChessManager.Domain.Models;

namespace ChessManager;

public static class MailTemplate
{
    
    public static string GetSubjectForNewMember(Member member)
    {
        return $"Welcome to ChessManager, {member.Pseudo}!";
    }

    public static string GetBodyForNewMember(Member member, string clearPassword)
    {
        return $"Hello {member.Pseudo},\n\n" +
               $"Welcome to ChessManager! We are glad to have you with us.\n\n" +
               $"You can now log in to your account with the following credentials:\n" +
               $"- Email: {member.Email}\n" +
               $"- Password: {clearPassword}\n";
        
    }
}