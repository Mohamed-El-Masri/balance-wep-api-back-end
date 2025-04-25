using balance.domain;

namespace balance.Repositories.Interfaces
{
    public interface IFavoriteRepository : IGenericRepository<Favorite>
    {
        // Get favorites by user
        Task<IEnumerable<Favorite>> GetByUserAsync(string userId);
        
        // Check if property is favorited by user
        Task<bool> IsFavoritedByUserAsync(int propertyId, string userId);
        
        // Get favorites with property details
        Task<IEnumerable<Favorite>> GetWithPropertyDetailsAsync(string userId);
    }
}
