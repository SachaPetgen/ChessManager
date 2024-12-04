using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Models;
using Isopoh.Cryptography.Argon2;

namespace ChessManager.Applications.Services;

public class MemberService : IMemberService
{
    
    private readonly IMemberRepository _memberRepository;
    
    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }
    
    public Task<Member?> GetByIdAsync(int id)
    {
        return _memberRepository.GetByIdAsync(id);
    }

    public Task<IEnumerable<Member>> GetAllAsync()
    {
        return _memberRepository.GetAllAsync();
    }

    public Task<Member?> CreateAsync(Member entity)
    {
        return _memberRepository.CreateAsync(entity);
    }
    
}