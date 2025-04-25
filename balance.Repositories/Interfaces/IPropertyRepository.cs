using balance.domain;
using balance.domain.Enums;

namespace balance.Repositories.Interfaces
{
    public interface IPropertyRepository : IGenericRepository<Property>
    {
        // Get properties by status
        Task<IEnumerable<Property>> GetByStatusAsync(Status status);
        
        // Get properties by type
        Task<IEnumerable<Property>> GetByPropertyTypeAsync(int propertyTypeId);
        
        // Get properties by price range
        Task<IEnumerable<Property>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        
        // Get properties by agent
        Task<IEnumerable<Property>> GetByAgentAsync(string agentId);
        
        // Get properties by project
        Task<IEnumerable<Property>> GetByProjectAsync(int projectId);
        
        // Get properties with images
        Task<IEnumerable<Property>> GetWithImagesAsync();
        
        // Get properties with features
        Task<IEnumerable<Property>> GetWithFeaturesAsync();
        
        // Get properties with offer type
        Task<IEnumerable<Property>> GetByOfferTypeAsync(offer_type offerType);
    }
}
