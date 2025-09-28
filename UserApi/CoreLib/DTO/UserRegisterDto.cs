namespace CoreLib.DTO;

public class UserRegisterDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string ConfirmPassword { get; set; }
}