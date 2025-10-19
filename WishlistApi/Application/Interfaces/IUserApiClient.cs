namespace Application.Interfaces;

public interface IUserApiClient
{
    Task<bool> UserExistsAsync(Guid userId);
    Task<UserInfoDto?> GetUserInfoAsync(Guid userId);
}

public class UserInfoDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = String.Empty;
}