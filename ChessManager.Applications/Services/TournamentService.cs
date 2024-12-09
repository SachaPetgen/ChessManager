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
    
    public TournamentService(ITournamentRepository tournamentRepository, IMailService mailService, IMemberRepository memberRepository)
    {
        _tournamentRepository = tournamentRepository;
        _mailService = mailService;
        _memberRepository = memberRepository;
    }

    public async Task<Tournament?> GetByIdAsync(int id)
    {
        Tournament? tournament = await _tournamentRepository.GetByIdAsync(id);


        if (tournament is null)
        {
            return null;
        }

        tournament.Members = await _tournamentRepository.GetMembers(id);

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
    
    private static bool IsAllowedToRegister(Member member, Tournament tournament)
    {
        return member.Elo >= tournament.MinEloAllowed && member.Elo <= tournament.MaxEloAllowed &&
               (tournament.WomenOnly == false || member.Gender == Gender.F);
    }
    
    private async Task SendMailToPlayersForNewTournament(Tournament tournament)
    {
        foreach (Member member in await _memberRepository.GetAllAsync())
        {
            if(IsAllowedToRegister(member, tournament))
            {
                _mailService.SendMail(member.Email, member.Pseudo, MailTemplate.GetSubjectForNewTournament(tournament), MailTemplate.GetBodyForNewTournament(tournament, member));
            }
        }
    }

    private async Task SendMailToPlayersForDeletedTournament(Tournament tournament)
    {
        foreach (Member member in await _memberRepository.GetAllAsync())
        {
            if(IsAllowedToRegister(member, tournament))
            {
                _mailService.SendMail(member.Email, member.Pseudo, MailTemplate.GetSubjectForNewTournament(tournament), MailTemplate.GetBodyForNewTournament(tournament, member));
            }
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
}