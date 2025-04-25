using balance.domain;
using balance.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.Repositories.Repositories
{
    public class PropertyTypeRepository : GenericRepository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(Context context) : base(context)
        {
        }

        public async Task<IEnumerable<PropertyType>> GetWithPropertiesAsync()
        {
            return await _dbSet
                .Where(pt => !pt.IsDeleted)
                .Include(pt => pt.Properties.Where(p => !p.IsDeleted))
                .ToListAsync();
        }
    }
}
