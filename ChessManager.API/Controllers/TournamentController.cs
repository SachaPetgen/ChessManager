using System.Security.Claims;
using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Exceptions;
using ChessManager.Domain.Models;
using ChessManager.DTO.Category;
using ChessManager.DTO.Member;
using ChessManager.DTO.Tournament;
using ChessManager.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChessManager.Controllers;


[Route("api/[controller]")]
[ApiController]

public class TournamentController : ControllerBase
{


    private readonly ITournamentService _tournamentService;

    public TournamentController(ITournamentService tournamentService)
    {
        _tournamentService = tournamentService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<TournamentViewListDTO>>> GetAllAsync()
    {
        try
        {
            IEnumerable<Tournament> tournaments = await _tournamentService.GetAllAsync();
            return Ok(tournaments.Select(m => m.ToTournamentViewListDto()));
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
    public async Task<ActionResult<TournamentViewDTO>> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            Tournament? tournament = await _tournamentService.GetByIdAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            return Ok(tournament.ToTournamentViewDto());
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


    [HttpGet("last-modified/{number:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<Tournament>>> GetLastModifiedAsync([FromRoute] int number)
    {
        try
        {
            IEnumerable<Tournament> tournaments = await _tournamentService.GetLastModified(number);
            return Ok(tournaments);
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
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    //[Authorize(Roles = "Admin")]
    public async Task<ActionResult<TournamentViewDTO>> CreateAsync([FromBody] TournamentCreateDTO createTournamentDTO)
    {
        
        try
        {
            Tournament tournament = createTournamentDTO.ToTournament();

            Tournament? createdTournament = await _tournamentService.CreateAsync(tournament);

            if (createdTournament is null)
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetByIdAsync),
                new
                {
                    id = createdTournament.Id
                },
                createdTournament.ToTournamentViewDto());
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

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        try
        {
            bool isDeleted = await _tournamentService.DeleteAsync(id);
            if (!isDeleted)
            {
                return NotFound();
            }
            return NoContent();
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

    [HttpPost("register-member")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<ActionResult> RegisterMember([FromBody] RegisterMemberTournamentDTO registerMemberDTO)
    {
        try
        {
            int memberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            bool isRegistered = await _tournamentService.RegisterMember(memberId, registerMemberDTO.TournamentId);
            if (!isRegistered)
            {
                return BadRequest();
            }
            return Ok(isRegistered);
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
    
    [HttpPost("unregister-member")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize]
    public async Task<ActionResult> UnregisterMember([FromBody] RegisterMemberTournamentDTO registerMemberDTO)
    {
        try
        {
            int memberId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            bool isUnregistered = await _tournamentService.UnregisterMember(memberId, registerMemberDTO.TournamentId);
            if (!isUnregistered)
            {
                return BadRequest();
            }
            return Ok(isUnregistered);
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

    [HttpPost("add-category")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> AddCategory([FromBody] AddCategoryToTournamentDTO addCategoryDTO)
    {
        try
        {
            bool result = await _tournamentService.AddCategory(addCategoryDTO.TournamentId, addCategoryDTO.CategoryId);
            return Ok(result);
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