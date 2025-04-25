using balance.domain;

namespace balance.Repositories.Interfaces
{
    public interface IPropertyTypeRepository : IGenericRepository<PropertyType>
    {
        // Get property type with properties
        Task<IEnumerable<PropertyType>> GetWithPropertiesAsync();
    }
}
