namespace CoreLib.DTO;

public class UserUpdateDto
{
    public string? Username { get; set; }
    public string? NewPassword { get; set; }
    public string? CurrentPassword { get; set; }
}