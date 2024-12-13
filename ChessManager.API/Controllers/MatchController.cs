using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChessManager.Controllers;



[Route("api/[controller]")]
[ApiController]

public class MatchController : ControllerBase
{
    
    
    private readonly IMatchService _matchService;

    public MatchController(IMatchService matchService)
    {
        _matchService = matchService;
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateResultAsync([FromRoute] int id, [FromBody] Result result)
    {
        try
        {
            bool updated = await _matchService.UpdateResult(id, result);
            if (!updated)
            {
                return NotFound();
            }
            return Ok();
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}