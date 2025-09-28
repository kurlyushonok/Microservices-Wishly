using CoreLib.Entities;
using CoreLib.Interfaces;

namespace Dal.Repositories;

public class UserRepository : IUserRepository
{
    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<User> GetByUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsUsernameAsync(string username)
    {
        throw new NotImplementedException();
    }
}