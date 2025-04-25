using balance.services.DTOs;

namespace balance.services.Interfaces
{
    public interface IFavoriteService
    {
        Task<IEnumerable<FavoriteDTO>> GetUserFavoritesAsync(string userId);
        Task<bool> IsFavoritedByUserAsync(int propertyId, string userId);
        Task<FavoriteDTO> AddToFavoritesAsync(int propertyId, string userId, string notes = null);
        Task<bool> RemoveFromFavoritesAsync(int propertyId, string userId);
        Task<bool> UpdateFavoriteNotesAsync(int favoriteId, string notes);
    }
}
