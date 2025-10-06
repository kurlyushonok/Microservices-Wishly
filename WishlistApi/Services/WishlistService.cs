using Application.Dto;
using Domain.Entities;
using Domain.Logic.Interfaces;
using Domain.Interfaces;

namespace Services;

public class WishlistService : IWishlistService
{
    private readonly IWishlistRepository _wishlistRepository;

    public WishlistService(IWishlistRepository wishlistRepository)
    {
        _wishlistRepository = wishlistRepository;
    }
    public async Task<WishlistResponseDto> CreateAsync(WishlistCreateDto createDto, Guid userId)
    {
        if (string.IsNullOrWhiteSpace(createDto.Title) || createDto.Title.Length > 100)
            throw new ArgumentException("The title must be between 1 and 100 characters long");
        if (createDto.Description.Length > 500)
            throw new ArgumentException("The title must be between 1 and 100 characters long");
        if (await _wishlistRepository.ExistsWithTitleAsync(createDto.Title, userId))
            throw new InvalidOperationException($"Wishlist with title '{createDto.Title}' already exists");

        var wishlist = new Wishlist
        {
            Id = Guid.NewGuid(),
            Title = createDto.Title,
            Description = createDto.Description,
            UserId = userId,
            CreatedAt = DateTime.Now
        };

        await _wishlistRepository.AddAsync(wishlist);

        return new WishlistResponseDto
        {
            Id = wishlist.Id,
            Title = wishlist.Title,
            Description = wishlist.Description
        };
    }

    public async Task<WishlistResponseDto> UpdateAsync(WishlistUpdateDto updateDto, Guid userId)
    {
        var wishlist = await _wishlistRepository.GetByIdAsync(updateDto.Id);
        if (wishlist == null)
        {
            throw new ArgumentException("Wishlist not found");
        }
        
        if (string.IsNullOrWhiteSpace(updateDto.NewTitle) || updateDto.NewTitle.Length > 100)
            throw new ArgumentException("The title must be between 1 and 100 characters long");

        if (updateDto.NewDescription?.Length > 500)
            throw new ArgumentException("Description must be less than 500 characters");
        
        wishlist.Title = updateDto.NewTitle.Trim();
        wishlist.Description = updateDto.NewDescription?.Trim();
        wishlist.CreatedAt = DateTime.UtcNow;

        await _wishlistRepository.UpdateAsync(wishlist);

        return new WishlistResponseDto
        {
            Id = wishlist.Id,
            Title = wishlist.Title,
            Description = wishlist.Description
        };
    }

    public async Task DeleteAsync(Guid id, Guid userId)
    {
        var wishlist = await _wishlistRepository.GetByIdAsync(id);
        if (wishlist == null)
        {
            return;
        }
        
        if (wishlist.UserId != userId)
            throw new UnauthorizedAccessException("You can only delete your own wishlists");
        
        await _wishlistRepository.DeleteAsync(id);
    }

    public async Task<List<WishlistResponseDto>?> GetAllAsync(Guid userId)
    {
        var wishlists = await _wishlistRepository.GetAllByUserIdAsync(userId);
        if (wishlists == null)
            return null;
        return wishlists
            .Select(w => new WishlistResponseDto
            {
                Id = w.Id,
                Title = w.Title,
                Description = w.Description
            }).ToList();
    }

    public async Task<WishlistResponseDto> GetByIdAsync(Guid id)
    {
        var wishlist = await _wishlistRepository.GetByIdAsync(id);
        if (wishlist == null)
        {
            throw new ArgumentException("Wishlist not found");
        }

        return new WishlistResponseDto
        {
            Id = wishlist.Id,
            Title = wishlist.Title,
            Description = wishlist.Description
        };
    }

    public async Task<WishlistResponseDto> GetByTitleAsync(string title)
    {
        var wishlist = await _wishlistRepository.GetByTitleAsync(title);
        if (wishlist == null)
        {
            throw new ArgumentException("Wishlist not found");
        }

        return new WishlistResponseDto
        {
            Id = wishlist.Id,
            Title = wishlist.Title,
            Description = wishlist.Description
        };
    }

    public async Task<bool> ExistsWithTitleAsync(string title, Guid id)
    {
        return await _wishlistRepository.ExistsWithTitleAsync(title, id);
    }
}