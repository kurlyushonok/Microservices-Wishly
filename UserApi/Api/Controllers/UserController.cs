using Dal.DTO;
using Logic.Interfaces;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Saga.Contracts;

namespace Api.Controllers;

[ApiController]
[Route("wishly/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IPublishEndpoint _publishEndpoint;

    public UserController(IUserService userService, IPublishEndpoint publishEndpoint)
    {
        _userService = userService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
    {
        try
        {
            var result = await _userService.RegisterAsync(registerDto);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserAuthDto authDto)
    {
        try
        {
            var result = await _userService.AuthenticateAsync(authDto);
            
            //TODO: генерация токена
            var token = "generated_jwt_token";
            return Ok(new
            {
                token = token,
                user = result
            });
        }
        catch (UnauthorizedAccessException  ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        try
        {
            var result = await _userService.GetByUsernameAsync(username);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound( new { error = ex.Message});
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var result = await _userService.GetByIdAsync(id);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound( new { error = ex.Message});
        }
    }
    
    [HttpPost("search")]
    public async Task<IActionResult> Search([FromBody] UserSearchDto searchDto)
    {
        try
        {
            var result = await _userService.GetByUsernameAsync(searchDto.Username);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return NotFound( new { error = ex.Message});
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _userService.DeleteAsync(id);
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateDto updateDto)
    {
        try
        {
            var result = await _userService.UpdateAsync(id, updateDto);
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
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = ex.Message });
        }
    }
    
    [HttpGet("{id}/exists")]
    public async Task<IActionResult> UserExists(Guid id)
    {
        try
        {
            var isExist = await _userService.IdExistsAsync(id);
            return Ok(new { exists = isExist });
        }
        catch (ArgumentException)
        {
            return Ok(new { exists = false });
        }
    }
    
    [HttpPost("register-with-saga")]
    public async Task<IActionResult> RegisterWithSaga([FromBody] UserRegisterDto registerDto)
    {
        try
        {
            var correlationId = Guid.NewGuid();
            
            await _publishEndpoint.Publish(new StartUserCreation
            {
                CorrelationId = correlationId,
                Username = registerDto.Username,
                Password = registerDto.Password,
                ConfirmPassword = registerDto.ConfirmPassword,
            });

            return Accepted(new 
            { 
                CorrelationId = correlationId,
                UserId = Guid.NewGuid(),
                Message = "User creation started", 
                Status = "Processing" 
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}