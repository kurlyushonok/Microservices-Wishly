using Application.Dto;
using Domain.Logic.Interfaces;

namespace Domain.Logic.Services;

public class WishlistService : IWishlistService
{
    public Task<WishlistResponseDto> CreateAsync(WishlistCreateDto createDto)
    {
        throw new NotImplementedException();
    }

    public Task<WishlistResponseDto> UpdateAsync(WishlistUpdateDto updateDto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<List<WishlistResponseDto>> GetAllAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<WishlistResponseDto> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<WishlistResponseDto> GetByTitleAsync(string title)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ExistsWithTitleAsync(string title, Guid id)
    {
        throw new NotImplementedException();
    }
}