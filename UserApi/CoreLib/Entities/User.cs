namespace CoreLib.Entities;

public class User : BaseEntityDal<Guid>
{
    public required string Username { get; set; }
    
    public required string PasswordHash { get; set; }
}