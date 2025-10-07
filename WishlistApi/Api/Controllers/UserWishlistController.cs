using Domain.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("wishly/users")]
public class UserWishlistController : ControllerBase
{
    private readonly IWishlistService _wishlistService;
    
    public UserWishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }
    
    [HttpGet("{userId}/wishlists")]
    public async Task<IActionResult> GetAllWishlistsById(Guid userId)
    {
        var result = await _wishlistService.GetAllAsync(userId);
        return Ok(result); 
    }
}