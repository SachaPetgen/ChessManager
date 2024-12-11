using ChessManager.Applications.Interfaces.Services;
using ChessManager.Domain.Models;
using ChessManager.DTO.Category;
using ChessManager.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace ChessManager.Controllers;

[Route("api/[controller]")]
[ApiController]

public class CategoryController : ControllerBase
{

    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CategoryViewListDTO>> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            Category? category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category.ToCategoryViewListDTO());
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
    public async Task<ActionResult<CategoryViewListDTO>> CreateAsync([FromBody] CategoryViewListDTO? categoryViewListDto)
    {
        if (categoryViewListDto is null || !this.ModelState.IsValid)
        {
            return BadRequest(new
            {
                message = "Invalid data"
            });
        }

        try
        {
            Category? category = await _categoryService.CreateAsync(categoryViewListDto.ToCategory());

            if (category is null)
            {
                return StatusCode(500, "Failed to create category.");
            }

            return CreatedAtAction(nameof(GetByIdAsync),
                new
                {
                    id = category.Id
                },
                category.ToCategoryViewListDTO());
        }
        catch (Exception e)
        {
            return Problem(e.Message);
        }
    }
}