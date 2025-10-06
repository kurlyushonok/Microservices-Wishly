using Domain.Entities;
using Infrastucture.Data;
using Infrastucture.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastucture.Repositories;

public class WishlistRepository : IWishlistRepository
{
    private readonly ApplicationDbContext _context;

    public WishlistRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(Wishlist wishlist)
    {
        await _context.Wishlists.AddAsync(wishlist);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Wishlist wishlist)
    {
        var existingWishlist = await _context.Wishlists.FindAsync(wishlist.Id);
        if (existingWishlist != null)
        {
            _context.Wishlists.Update(wishlist);
            await _context.SaveChangesAsync();
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var wishlist = await _context.Wishlists.FindAsync(id);
        if (wishlist != null)
        {
            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<Wishlist>?> GetAllByUserIdAsync(Guid userId)
    {
        return await _context.Wishlists
            .Where(w => w.UserId == userId)
            .OrderBy(w => w.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Wishlist?> GetByIdAsync(Guid id)
    {
        return await _context.Wishlists.FindAsync(id);
    }

    public async Task<Wishlist?> GetByTitleAsync(string title)
    {
        return await _context.Wishlists
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.Title == title);
    }

    public async Task<bool> ExistsWithTitleAsync(string title, Guid userId)
    {
        return await _context.Wishlists
            .AsNoTracking()
            .AnyAsync(w => w.UserId == userId && w.Title == title);
    }
}