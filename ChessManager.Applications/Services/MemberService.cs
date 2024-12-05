using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Models;
using ChessManager.Infrastructure.Mail;
using Isopoh.Cryptography.Argon2;

namespace ChessManager.Applications.Services;

public class MemberService : IMemberService
{
    
    private readonly IMemberRepository _memberRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMailService _mailService;
    
    public MemberService(IMemberRepository memberRepository, IPasswordService passwordService, IMailService mailService)
    {
        _memberRepository = memberRepository;
        _passwordService = passwordService;
        _mailService = mailService;
    }
    
    public Task<Member?> GetByIdAsync(int id)
    {
        return _memberRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<Member>> GetAllAsync()
    {
        return _memberRepository.GetAllAsync();
    }

    public async Task<Member?> CreateAsync(Member entity)
    {
        string randomPassword = _passwordService.GenerateRandomPassword();
        entity.Password = _passwordService.HashPassword(randomPassword);
        
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;

        entity.Elo ??= 1200;
        
        Member? member = await _memberRepository.CreateAsync(entity);
        
        _mailService.SendMail(entity.Email, entity.Pseudo, MailTemplate.GetSubjectForNewMember(entity), MailTemplate.GetBodyForNewMember(entity, randomPassword));
        return member;
    }
    
}