using System.Security.Claims;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Exceptions;
using ChessManager.Domain.Models;
using ChessManager.DTO.Member;
using ChessManager.Infrastructure.Mail;
using ChessManager.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable ConvertIfStatementToReturnStatement

namespace ChessManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MemberController : ControllerBase
{
    
    private readonly IMemberService _memberService;
    
    public MemberController(IMemberService memberService)
    {
        _memberService = memberService;
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
    [Authorize]
    public async Task<ActionResult<MemberCreateDTO>> CreateAsync([FromBody] MemberCreateDTO? memberCreateDTO)
    {
        if (memberCreateDTO is null || !this.ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid data" });
        }
        
        try
        {
            Member? member = await _memberService.CreateAsync(memberCreateDTO.ToMember());
            if (member is null)
            {
                return StatusCode(500, "Failed to create member.");
            }
            
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
    
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<ActionResult<string>> Login([FromBody] MemberLoginDTO? memberLoginDTO)
    {
        if (memberLoginDTO is null || !this.ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid data" });
        }
        
        try
        {
            return await _memberService.Login(memberLoginDTO.Identifier, memberLoginDTO.Password);
            
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
    
    [HttpPost("change-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<ActionResult> ChangePassword([FromBody] MemberChangePasswordDTO? memberChangePasswordDTO)
    {
        
        if (memberChangePasswordDTO is null || !this.ModelState.IsValid)
        {
            return BadRequest(new { message = "Invalid data" });
        }
        
        try
        {
            ClaimsPrincipal principal = this.User;
            int memberId = int.Parse(principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "-1");
            
            await _memberService.ChangePassword(memberId, memberChangePasswordDTO.Password, memberChangePasswordDTO.ConfirmPassword, memberChangePasswordDTO.OldPassword);
            return Ok();
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