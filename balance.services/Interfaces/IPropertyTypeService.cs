using balance.services.DTOs;

namespace balance.services.Interfaces
{
    public interface IPropertyTypeService
    {
        Task<IEnumerable<PropertyTypeDTO>> GetAllPropertyTypesAsync();
        Task<PropertyTypeDTO> GetPropertyTypeByIdAsync(int id);
        Task<PropertyTypeDTO> CreatePropertyTypeAsync(PropertyTypeDTO propertyTypeDTO);
        Task<PropertyTypeDTO> UpdatePropertyTypeAsync(PropertyTypeDTO propertyTypeDTO);
        Task<bool> DeletePropertyTypeAsync(int id);
        Task<IEnumerable<PropertyTypeDTO>> GetPropertyTypesWithCountsAsync();
    }
}
