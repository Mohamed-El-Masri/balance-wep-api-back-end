using balance.domain;
using balance.domain.Enums;
using balance.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.Repositories.Repositories
{
    public class PropertyRepository : GenericRepository<Property>, IPropertyRepository
    {
        public PropertyRepository(Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Property>> GetByAgentAsync(string agentId)
        {
            return await _dbSet
                .Where(p => p.AgentId == agentId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetByOfferTypeAsync(offer_type offerType)
        {
            return await _dbSet
                .Where(p => p.offer_type == offerType && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _dbSet
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetByProjectAsync(int projectId)
        {
            return await _dbSet
                .Where(p => p.ProjectId == projectId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetByPropertyTypeAsync(int propertyTypeId)
        {
            return await _dbSet
                .Where(p => p.PropertyTypeId == propertyTypeId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetByStatusAsync(Status status)
        {
            return await _dbSet
                .Where(p => p.Status == status && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetWithFeaturesAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .Include(p => p.Features)
                .ToListAsync();
        }

        public async Task<IEnumerable<Property>> GetWithImagesAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .Include(p => p.Images)
                .ToListAsync();
        }
    }
}
