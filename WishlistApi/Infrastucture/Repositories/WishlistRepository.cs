using Domain.Entities;
using Infrastucture.Interfaces;

namespace Infrastucture.Repositories;

public class WishlistRepository : IWishlistRepository
{
    public Task AddAsync(Wishlist wishlist)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Wishlist wishlist)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Wishlist wishlist)
    {
        throw new NotImplementedException();
    }

    public Task<Wishlist[]> GetAllByUserIdAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<Wishlist> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Wishlist> GetByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsWithTitleAsync(string title, Guid userId)
    {
        throw new NotImplementedException();
    }
}