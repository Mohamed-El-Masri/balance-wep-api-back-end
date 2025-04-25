using balance.domain;
using balance.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.Repositories.Repositories
{
    public class FavoriteRepository : GenericRepository<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Favorite>> GetByUserAsync(string userId)
        {
            return await _dbSet
                .Where(f => f.UserId == userId && !f.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> IsFavoritedByUserAsync(int propertyId, string userId)
        {
            return await _dbSet.AnyAsync(f => f.PropertyId == propertyId && 
                                             f.UserId == userId && 
                                             !f.IsDeleted);
        }

        public async Task<IEnumerable<Favorite>> GetWithPropertyDetailsAsync(string userId)
        {
            return await _dbSet
                .Where(f => f.UserId == userId && !f.IsDeleted)
                .Include(f => f.Property)
                    .ThenInclude(p => p.PropertyType)
                .Include(f => f.Property)
                    .ThenInclude(p => p.Images)
                .ToListAsync();
        }
    }
}
