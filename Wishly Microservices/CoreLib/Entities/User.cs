using System;

namespace CoreLib.Entities;

public class User : BaseEntityDal<Guid>
{
    public required Guid Id { get; init; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
}