using CoreLib.DTO;
using CoreLib.Entities;
using CoreLib.Interfaces;

namespace Logic.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public async Task<UserResponseDto> RegisterAsync(UserRegisterDto registerDto)
    {
        if (string.IsNullOrWhiteSpace(registerDto.Username) ||
            registerDto.Username.Length < 3 || registerDto.Username.Length > 50)
            throw new ArgumentException("The name must be between 3 and 50 characters long");
        
        if (string.IsNullOrWhiteSpace(registerDto.Password) ||
            registerDto.Password.Length < 8 || registerDto.Password.Length > 50)
            throw new ArgumentException("The password must be between 8 and 50 characters long");

        if (registerDto.Password != registerDto.ConfirmPassword)
            throw new ArgumentException("Passwords do not match");

        if (await _userRepository.ExistsUsernameAsync(registerDto.Username))
            throw new InvalidOperationException("Username already exists");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = registerDto.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password)
        };

        await _userRepository.AddAsync(user);

        return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username
            };
    }

    public async Task<UserResponseDto> AuthenticateAsync(UserAuthDto authDto)
    {
        var user = await _userRepository.GetByUsernameAsync(authDto.Username);
        if (user == null || !BCrypt.Net.BCrypt.Verify(authDto.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid username or password");
        
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username
        };
    }

    public async Task<UserResponseDto> GetByUsernameAsync(string username)
    {
        var user = await _userRepository.GetByUsernameAsync(username);
        if (user == null)
            throw new ArgumentException("User not found");
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
        };
    }

    public async Task<UserResponseDto> GetByIdAsync(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new ArgumentException("User not found");
        
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
        };
    }

    public async Task<UserResponseDto> UpdateAsync(Guid id, UserUpdateDto updateDto)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
            throw new ArgumentException("User not found");

        if (UsernameUpdateAsync(user, updateDto).Result || PasswordUpdateAsync(user, updateDto))
        {
            await _userRepository.UpdateAsync(user);
        }
        
        return new UserResponseDto
        {
            Id = user.Id,
            Username = user.Username,
        };
    }

    private async Task<bool> UsernameUpdateAsync(User user, UserUpdateDto updateDto)
    {
        if (!string.IsNullOrWhiteSpace(updateDto.Username) && updateDto.Username != user.Username)
        {
            if (await _userRepository.ExistsUsernameAsync(updateDto.Username))
                throw new InvalidOperationException("Username already exists");
            
            user.Username = updateDto.Username;
            return true;
        }

        return false;
    }

    private bool PasswordUpdateAsync(User user, UserUpdateDto updateDto)
    {
        if (!string.IsNullOrWhiteSpace(updateDto.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(updateDto.CurrentPassword))
                throw new ArgumentException("Current password is required to change password");
            if (!BCrypt.Net.BCrypt.Verify(updateDto.CurrentPassword, user.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect");
            if (updateDto.NewPassword.Length < 8 || updateDto.NewPassword.Length > 50)
                throw new ArgumentException("The password must be between 8 and 50 characters long");

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(updateDto.NewPassword);
            return true;
        }

        return false;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _userRepository.DeleteAsync(id);
    }

    public async Task<bool> UsernameExistsAsync(string username)
    {
        return await _userRepository.ExistsUsernameAsync(username);
    }
}