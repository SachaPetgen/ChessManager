using ChessManager.Domain.Models;
using ChessManager.DTO.Tournament;

namespace ChessManager.Mappers;

public static class TournamentMapper
{
    public static TournamentViewListDTO ToTournamentViewListDto(this Tournament tournament)
    {
        return new TournamentViewListDTO()
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Location = tournament.Location,
            MaxPlayerCount = tournament.MaxPlayerCount,
            MaxEloAllowed = tournament.MaxEloAllowed,
            Status = tournament.Status
        };
    }

    public static TournamentViewDTO ToTournamentViewDto(this Tournament tournament)
    {
        return new TournamentViewDTO()
        {
            Id = tournament.Id,
            Name = tournament.Name,
            Location = tournament.Location,
            MaxPlayerCount = tournament.MaxPlayerCount,
            MinPlayerCount = tournament.MinPlayerCount,
            MaxEloAllowed = tournament.MaxEloAllowed,
            MinEloAllowed = tournament.MinEloAllowed,
            Status = tournament.Status,
            CurrentRound = tournament.CurrentRound,
            WomenOnly = tournament.WomenOnly,
            RegistrationEndDate = tournament.RegistrationEndDate
        };
    }

    public static Tournament ToTournament(this TournamentCreateDTO tournamentCreateDto)
    {
        return new Tournament()
        {
            Name = tournamentCreateDto.Name,
            Location = tournamentCreateDto.Location,
            MaxPlayerCount = tournamentCreateDto.MaxPlayerCount,
            MinPlayerCount = tournamentCreateDto.MinPlayerCount,
            MaxEloAllowed = tournamentCreateDto.MaxEloAllowed,
            MinEloAllowed = tournamentCreateDto.MinEloAllowed,
            WomenOnly = tournamentCreateDto.WomenOnly,
            RegistrationEndDate = tournamentCreateDto.RegistrationEndDate
        };
    }
}