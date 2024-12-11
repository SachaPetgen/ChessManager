using ChessManager.Applications.Interfaces.Repo;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Exceptions;
using ChessManager.Domain.Models;
using ChessManager.Infrastructure.Mail;

namespace ChessManager.Applications.Services;

public class TournamentService : ITournamentService
{
    
    private readonly ITournamentRepository _tournamentRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IMailService _mailService;
    private readonly ICategoryRepository _categoryRepository;
    
    public TournamentService(ITournamentRepository tournamentRepository, IMailService mailService, IMemberRepository memberRepository, ICategoryRepository categoryRepository)
    {
        _tournamentRepository = tournamentRepository;
        _mailService = mailService;
        _memberRepository = memberRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Tournament?> GetByIdAsync(int id)
    {
        Tournament? tournament = await _tournamentRepository.GetByIdAsync(id);


        if (tournament is null)
        {
            return null;
        }

        tournament.Members = await _tournamentRepository.GetMembers(id);
        tournament.Categories = await _tournamentRepository.GetCategories(id);

        return tournament;
    }

    public Task<IEnumerable<Tournament>> GetAllAsync()
    {
        return _tournamentRepository.GetAllAsync();
    }

    public Task<IEnumerable<Tournament>> GetLastModified(int number)
    {
        return _tournamentRepository.GetLastModified(number);
    }
    
    private async Task<bool> IsAllowedToRegister(Member member, Tournament tournament)
    {
        int memberAge = (tournament.RegistrationEndDate - member.BirthDate).Days / 365;

        return tournament.Status == Status.PENDING
               && tournament.RegistrationEndDate >= DateTime.Now
               && !await _tournamentRepository.CheckIfMemberIsRegistered(member.Id, tournament.Id)
               && tournament.MaxPlayerCount > await _tournamentRepository.GetNumberOfRegisteredMembers(tournament.Id)
               && (await _tournamentRepository.GetCategories(tournament.Id)).Select(c => memberAge <= c.AgeMax && memberAge >= c.AgeMin).Any()
               && (member.Elo > tournament.MinEloAllowed || member.Elo < tournament.MaxEloAllowed)
               && (!tournament.WomenOnly || member.Gender != Gender.M);

    }
    
    private async Task SendMailToPlayersForNewTournament(Tournament tournament)
    {
        foreach (Member member in await _memberRepository.GetAllAsync())
        {
            if(await IsAllowedToRegister(member, tournament))
            {
                _mailService.SendMail(member.Email, member.Pseudo, MailTemplate.GetSubjectForNewTournament(tournament), MailTemplate.GetBodyForNewTournament(tournament, member));
            }
        }
    }

    private async Task SendMailToPlayersForDeletedTournament(Tournament tournament)
    {
        foreach (Member member in await _tournamentRepository.GetMembers(tournament.Id))
        {
            _mailService.SendMail(member.Email, member.Pseudo, MailTemplate.GetSubjectForNewTournament(tournament), MailTemplate.GetBodyForNewTournament(tournament, member));
        }
    }
    
    public async Task<Tournament?> CreateAsync(Tournament entity)
    {
        entity.CurrentRound = 0;
        
        entity.Status = Status.PENDING;
        
        entity.CreatedAt = DateTime.Now;
        entity.UpdatedAt = DateTime.Now;
        
        if (entity.RegistrationEndDate < DateTime.Now.AddDays(entity.MinPlayerCount))
        {
            throw new BadRegistrationEndDateException();
        }
        
        Tournament? tournament = await _tournamentRepository.CreateAsync(entity);

        if (tournament is not null)
        {
            await SendMailToPlayersForNewTournament(tournament);
        }
        
        return tournament;
    }
    
    public async Task<bool> DeleteAsync(int id)
    {
        Tournament? tournament = await GetByIdAsync(id);
        
        if (tournament is not { Status: Status.PENDING })
        {
            throw new UnableToDeleteTournamentException();
        }

        foreach (Member member in await _tournamentRepository.GetMembers(id))
        {
            await SendMailToPlayersForDeletedTournament(tournament);
        }
        
        return await _tournamentRepository.DeleteAsync(id);
    }

    public async Task<bool> RegisterMember(int memberId, int tournamentId)
    {
        Member? member = await _memberRepository.GetByIdAsync(memberId);
        Tournament? tournament = await GetByIdAsync(tournamentId);

        if (member is null || tournament is null)
        {
            throw new InvalidIdentifierException();
        }

        if (!await IsAllowedToRegister(member, tournament))
        {
            throw new UnableToRegisterMemberException();
        }

        return await _tournamentRepository.RegisterMember(memberId, tournamentId);
    }
    
    public async Task<bool> UnregisterMember(int memberId, int tournamentId)
    {
        Member? member = await _memberRepository.GetByIdAsync(memberId);
        Tournament? tournament = await GetByIdAsync(tournamentId);

        if (member is null || tournament is null)
        {
            throw new InvalidIdentifierException();
        }
        
        if(tournament.Status != Status.PENDING && !await _tournamentRepository.CheckIfMemberIsRegistered(memberId, tournamentId))
        {
            throw new MemberNotRegisteredException();
        }

        return await _tournamentRepository.UnregisterMember(memberId, tournamentId);
    }

    public async Task<bool> AddCategory(int tournamentId, int categoryId)
    {
        Category? category = await _categoryRepository.GetByIdAsync(categoryId);
        Tournament? tournament = await _tournamentRepository.GetByIdAsync(tournamentId);
        
        if (category is null || tournament is null)
        {
            throw new InvalidIdentifierException();
        }

        return await _tournamentRepository.AddCategory(tournamentId, categoryId);

    }
    
    private async Task<bool> IsAbleToStart(Tournament tournament)
    {

        return tournament.Status == Status.PENDING
               && tournament.RegistrationEndDate <= DateTime.Now
               && await _tournamentRepository.GetNumberOfRegisteredMembers(tournament.Id) >= tournament.MinPlayerCount;
    }

    public async Task<bool> CreateMatchmaking(Tournament tournament)
    {

        IEnumerable<Member> members = await _tournamentRepository.GetMembers(tournament.Id);

        int numberOfPlayers = members.Count();

        int numberOfRounds = numberOfPlayers - 1;

        int numberOfMatches = numberOfRounds * (numberOfPlayers / 2) * 2;
        

        for (int i = 0; i < numberOfRounds; i++)
        {
            // 1 2 3 4 5 6 7 8
            // 1 8 2 7 3 6 4 5
            // 1 7 8 6 2 5 3 4
            // 1 6 7 5 8 4 2 3
        }


    }
    
    public async Task<bool> StartTournament(int tournamentId)
    {
        Tournament? tournament = await GetByIdAsync(tournamentId);

        if (tournament is null)
        {
            throw new InvalidIdentifierException();
        }

        if (!await IsAbleToStart(tournament))
        {
            throw new TournamentUnableToStart();
        }
        
        
        

        return await _tournamentRepository.StartTournament(tournament.Id);

    }
}