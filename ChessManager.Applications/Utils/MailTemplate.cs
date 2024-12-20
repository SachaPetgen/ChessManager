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
    
    public static string GetSubjectForNewTournament(Tournament tournament)
    {
        return "New tournament";
    }
    
    public static string GetBodyForNewTournament(Tournament tournament, Member member)
    {
        return $"Hello {member.Pseudo},\n\nA new tournament has been created: {tournament.Name}.\n\nYou can now register to this tournament.\n\nGood luck!";
    }
    
    public static string GetSubjectForResetPassword(Member member)
    {
        return "Reset your password";
    }
    
    public static string GetBodyForResetPassword(Member member, string linkToReset)
    {
        return $"Hello {member.Pseudo},\n\n" +
               $"You have requested to reset your password. Here is the link to reset it:\n" +
               $"- {linkToReset}\n\n" +
               $"If you did not request this, please ignore this email.";
        
    }
    
}