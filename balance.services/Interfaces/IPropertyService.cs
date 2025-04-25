using balance.domain.Enums;
using balance.services.DTOs;

namespace balance.services.Interfaces
{
    public interface IPropertyService
    {
        Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync();
        Task<PropertyDTO> GetPropertyByIdAsync(int id);
        Task<IEnumerable<PropertyDTO>> GetPropertiesByStatusAsync(Status status);
        Task<IEnumerable<PropertyDTO>> GetPropertiesByTypeAsync(int propertyTypeId);
        Task<IEnumerable<PropertyDTO>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        Task<IEnumerable<PropertyDTO>> GetPropertiesByAgentAsync(string agentId);
        Task<IEnumerable<PropertyDTO>> GetPropertiesByProjectAsync(int projectId);
        Task<IEnumerable<PropertyDTO>> GetPropertiesByOfferTypeAsync(offer_type offerType);
        Task<PropertyDTO> CreatePropertyAsync(PropertyDTO propertyDTO);
        Task<PropertyDTO> UpdatePropertyAsync(PropertyDTO propertyDTO);
        Task<bool> DeletePropertyAsync(int id);
        Task<bool> TogglePropertyStatusAsync(int id);
    }
}
