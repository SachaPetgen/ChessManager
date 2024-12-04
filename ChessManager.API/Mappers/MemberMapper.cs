using ChessManager.Domain.Models;
using ChessManager.DTO.Member;

namespace ChessManager.Mappers;

public static class MemberMapper
{
    
    public static MemberViewListDTO ToMemberViewListDto(this Member member)
    {
        return new MemberViewListDTO()
        {
            Id = member.Id,
            Pseudo = member.Pseudo,
            Email = member.Email,
            Role = member.Role,
            Gender = member.Gender,
            Elo = member.Elo,
            BirthDate = member.BirthDate,
        }; 
    }

    public static Member ToMember(this MemberCreateDTO memberCreateDto)
    {
        return new Member()
        {
            Pseudo = memberCreateDto.Pseudo,
            Email = memberCreateDto.Email,
            Role = memberCreateDto.Role,
            Gender = memberCreateDto.Gender,
            Elo = memberCreateDto.Elo == 0 ? 1200 : memberCreateDto.Elo,
            BirthDate = memberCreateDto.BirthDate
        };
    }

}