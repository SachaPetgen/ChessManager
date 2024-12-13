using ChessManager.Applications.DTO;
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
    private readonly IMatchRepository _matchRepository;
    
    public TournamentService(ITournamentRepository tournamentRepository, IMailService mailService,
        IMemberRepository memberRepository, ICategoryRepository categoryRepository, IMatchRepository matchRepository)
    {
        _tournamentRepository = tournamentRepository;
        _mailService = mailService;
        _memberRepository = memberRepository;
        _categoryRepository = categoryRepository;
        _matchRepository = matchRepository;
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
        
        //check
        
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

    private async Task<bool> CreateMatchmaking(Tournament? tournament)
    {
        
        if (tournament is null)
            return false;
        
        List<Member?> members = (await _tournamentRepository.GetMembers(tournament.Id)).ToList()!;

        int numberOfPlayers = members.Count;

        int numberOfRounds = numberOfPlayers - 1;
        
        if (numberOfPlayers % 2 != 0)
        {
            members.Add(null);
        }
        
        for (int i = 0; i < numberOfRounds; i++)
        {
            for (int j = 0; j < numberOfPlayers; j += 2)
            {
                Member? whitePlayer = members[j];
                Member? blackPlayer = members[j + 1];

                Match? match;

                try
                {
                    if (whitePlayer is null || blackPlayer is null)
                        match = await _matchRepository.CreateMatchAsync(Result.ODDGAME, tournament.Id, blackPlayer?.Id,
                            whitePlayer?.Id);
                    else
                        match = await _matchRepository.CreateMatchAsync(Result.PENDING, tournament.Id, blackPlayer.Id,
                            whitePlayer.Id);

                    await _matchRepository.AddMatchToTournamentAsync(tournament.Id, match!.Id, i + 1);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }   
            
            Member? last = members[^1];
            members.RemoveAt(members.Count - 1);
            members.Insert(1, last);
        }
        
        return true;
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

        if(!await CreateMatchmaking(tournament))
        {
            throw new UnableToCreateMatchmaking();
        }

        return await _tournamentRepository.StartTournament(tournament.Id);
    }

    public async Task<bool> StartNextRound(int tournamentId)
    {
        return await _tournamentRepository.StartNextRound(tournamentId);
    }

    public async Task<List<ResultsDTO>> GetResults(int tournamentId)
    {
        
        Tournament tournament = await GetByIdAsync(tournamentId) ?? throw new InvalidIdentifierException();
        
        if(tournament.Status != Status.OVER)
            throw new TournamentNotFinishedException();
        
        IEnumerable<Member> members = await _tournamentRepository.GetMembers(tournamentId);

        IEnumerable<Match> matchs = await _tournamentRepository.GetMatches(tournamentId);
        
        List<ResultsDTO> results = [];

        foreach (Member member in members)
        {

            double score = 0;
            int win = 0;
            int lose = 0;
            int draw = 0;
            int total = 0;
            
            foreach (Match match in matchs)
            {

                if (match.BlackPlayerId == member.Id || match.WhitePLayerId == member.Id)
                {
                    total++;

                    switch (match.Result)
                    {
                        case Result.DRAW:
                            draw++;
                            score += 0.5;
                            break;
                        case Result.WHITEWINS:
                            if (match.WhitePLayerId == member.Id)
                            {
                                win++;
                                score += 1;
                            }
                            else
                            {
                                lose++;
                            }
                            break;
                        case Result.BLACKWINS:
                            if (match.BlackPlayerId == member.Id)
                            {
                                win++;
                                score += 1;
                            }
                            else
                            {
                                lose++;
                            }
                            break;
                    }
                }

            }
            results.Add(new ResultsDTO()
            {
                Score = score,
                VictoryNb = win,
                LossNb = lose,
                DrawNb = draw,
                TotalNb = total,
                MemberName = member.Pseudo
            });
            
        }
        
        return results;

    }
}