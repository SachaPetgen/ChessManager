using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Exceptions;
using ChessManager.Domain.Models;
using ChessManager.DTO.Member;
using ChessManager.Infrastructure.Mail;
using ChessManager.Mappers;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace ChessManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{


    private readonly IMemberService _memberService;

    private readonly IMailService _mailService;

    private readonly IPasswordService _passwordService;

    public MemberController(IMemberService memberService, IMailService mailService, IPasswordService passwordService)
    {
        _memberService = memberService;
        _mailService = mailService;
        _passwordService = passwordService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MemberViewListDTO>>> GetAllAsync()
    {
        try
        {
            IEnumerable<Member> members = await _memberService.GetAllAsync();
            return Ok(members.Select(m => m.ToMemberViewListDto()));
        }
        catch (DbErrorException e)
        {
            return StatusCode(500, e.Message);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Member>> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            Member? member = await _memberService.GetByIdAsync(id);
            return member is not null
                ? Ok(member.ToMemberViewListDto())
                : NotFound($"The id [{id}] doesn't exist in database!");
        }
        catch (DbErrorException e)
        {
            return StatusCode(500, e.Message);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MemberCreateDTO>> CreateAsync([FromBody] MemberCreateDTO? memberCreateDTO)
    {
        if (memberCreateDTO is null || !this.ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid data" });
        }
        
        string randomPassword = _passwordService.GenerateRandomPassword();
        
        try
        {
            Member? memberToAdd = memberCreateDTO.ToMember();
            memberToAdd.Password = _passwordService.HashPassword(randomPassword);
            
            Member? member = await _memberService.CreateAsync(memberToAdd);
            if (member is null)
            {
                return StatusCode(500, "Failed to create member.");
            }
            _mailService.SendMail(member.Email, member.Pseudo, MailTemplate.GetSubjectForNewMember(member), MailTemplate.GetBodyForNewMember(member, randomPassword));
            
            return CreatedAtAction(nameof(GetByIdAsync), new { id = member.Id }, member.ToMemberViewListDto());
        }
        catch (DbErrorException e)
        {
            return StatusCode(500, e.Message);
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}