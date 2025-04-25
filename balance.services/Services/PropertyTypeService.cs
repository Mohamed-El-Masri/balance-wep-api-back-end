using AutoMapper;
using balance.domain;
using balance.Repositories.Interfaces;
using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.services.Services
{
    public class PropertyTypeService : IPropertyTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PropertyTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyTypeDTO>> GetAllPropertyTypesAsync()
        {
            var propertyTypes = await _unitOfWork.PropertyTypes.GetAll()
                .Where(pt => !pt.IsDeleted)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<PropertyTypeDTO>>(propertyTypes);
        }

        public async Task<PropertyTypeDTO> GetPropertyTypeByIdAsync(int id)
        {
            var propertyType = await _unitOfWork.PropertyTypes.GetByIdAsync(id);
            if (propertyType == null || propertyType.IsDeleted)
                return null;
                
            return _mapper.Map<PropertyTypeDTO>(propertyType);
        }

        public async Task<PropertyTypeDTO> CreatePropertyTypeAsync(PropertyTypeDTO propertyTypeDTO)
        {
            var propertyType = _mapper.Map<PropertyType>(propertyTypeDTO);
            propertyType.CreatedAt = DateTime.Now;
            
            await _unitOfWork.PropertyTypes.AddAsync(propertyType);
            await _unitOfWork.CompleteAsync();
            
            return _mapper.Map<PropertyTypeDTO>(propertyType);
        }

        public async Task<PropertyTypeDTO> UpdatePropertyTypeAsync(PropertyTypeDTO propertyTypeDTO)
        {
            var existingPropertyType = await _unitOfWork.PropertyTypes.GetByIdAsync(propertyTypeDTO.Id);
            if (existingPropertyType == null)
                return null;

            _mapper.Map(propertyTypeDTO, existingPropertyType);
            existingPropertyType.ModifiedAt = DateTime.Now;
            
            _unitOfWork.PropertyTypes.Update(existingPropertyType);
            await _unitOfWork.CompleteAsync();
            
            return _mapper.Map<PropertyTypeDTO>(existingPropertyType);
        }

        public async Task<bool> DeletePropertyTypeAsync(int id)
        {
            // Check if property type is used by properties
            var hasProperties = await _unitOfWork.Properties.GetAll(p => p.PropertyTypeId == id).AnyAsync();
            if (hasProperties)
                return false; // Cannot delete if there are associated properties

            await _unitOfWork.PropertyTypes.DeleteAsync(id);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<IEnumerable<PropertyTypeDTO>> GetPropertyTypesWithCountsAsync()
        {
            var propertyTypes = await _unitOfWork.PropertyTypes.GetWithPropertiesAsync();
            return _mapper.Map<IEnumerable<PropertyTypeDTO>>(propertyTypes);
        }
    }
}
