using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Exceptions;
using ChessManager.Domain.Models;
using ChessManager.Infrastructure.Mail;
using Isopoh.Cryptography.Argon2;

namespace ChessManager.Applications.Services;

public class MemberService : IMemberService
{
    
    private readonly IMemberRepository _memberRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMailService _mailService;
    private readonly ITokenService _tokenService;
    
    public MemberService(IMemberRepository memberRepository, IPasswordService passwordService, IMailService mailService, ITokenService tokenService)
    {
        _memberRepository = memberRepository;
        _passwordService = passwordService;
        _mailService = mailService;
        _tokenService = tokenService;
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

    public async Task<string> Login(string identifier, string password)
    {
        
        Member? member = await _memberRepository.GetByEmail(identifier);
        
        if (member is null)
        {
            member = await _memberRepository.GetByPseudo(identifier);
        }
        
        if (member is null)
        {
            throw new InvalidIdentifierException();
        }

        Console.WriteLine($"{member.Password} {_passwordService.HashPassword(password)}");
        
        if (!_passwordService.VerifyPassword(member.Password, password))
        {
            throw new InvalidPasswordException();
        }
        
        return _tokenService.GenerateToken(member);
    }
    
}