using Application.Dto;
using Domain.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("wishly/wishlists")]
public class WishlistController  : ControllerBase
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] WishlistCreateDto createDto, Guid userId)
    {
        try
        {
            var result = await _wishlistService.CreateAsync(createDto, userId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromBody] WishlistUpdateDto updateDto, Guid userId)
    {
        try
        {
            var result = await _wishlistService.UpdateAsync(updateDto, userId);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid wishlistId, Guid userId)
    {
        try
        {
            await _wishlistService.DeleteAsync(wishlistId, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _wishlistService.GetAllAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpGet("{title}")]
    public async Task<IActionResult> GetByTitle(string title)
    {
        try
        {
            var result = await _wishlistService.GetByTitleAsync(title);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}