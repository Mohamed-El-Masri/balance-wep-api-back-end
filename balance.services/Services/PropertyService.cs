using AutoMapper;
using balance.domain;
using balance.domain.Enums;
using balance.Repositories.Interfaces;
using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.services.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PropertyService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PropertyDTO>> GetAllPropertiesAsync()
        {
            var properties = await _unitOfWork.Properties.GetAll(includeProperties: "PropertyType,Project,Agent,Images,Features")
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<PropertyDTO> GetPropertyByIdAsync(int id)
        {
            var property = await _unitOfWork.Properties.GetAll(
                    p => p.Id == id,
                    null,
                    "PropertyType,Project,Agent,Images,Features,GeoLocation")
                .FirstOrDefaultAsync();

            if (property == null)
                return null;

            return _mapper.Map<PropertyDTO>(property);
        }

        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByStatusAsync(Status status)
        {
            var properties = await _unitOfWork.Properties.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByTypeAsync(int propertyTypeId)
        {
            var properties = await _unitOfWork.Properties.GetByPropertyTypeAsync(propertyTypeId);
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            var properties = await _unitOfWork.Properties.GetByPriceRangeAsync(minPrice, maxPrice);
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByAgentAsync(string agentId)
        {
            var properties = await _unitOfWork.Properties.GetByAgentAsync(agentId);
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByProjectAsync(int projectId)
        {
            var properties = await _unitOfWork.Properties.GetByProjectAsync(projectId);
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<IEnumerable<PropertyDTO>> GetPropertiesByOfferTypeAsync(offer_type offerType)
        {
            var properties = await _unitOfWork.Properties.GetByOfferTypeAsync(offerType);
            return _mapper.Map<IEnumerable<PropertyDTO>>(properties);
        }

        public async Task<PropertyDTO> CreatePropertyAsync(PropertyDTO propertyDTO)
        {
            var property = _mapper.Map<Property>(propertyDTO);
            
            // Set default values for new property
            property.PostedDate = DateTime.Now;
            property.CreatedAt = DateTime.Now;
            
            await _unitOfWork.Properties.AddAsync(property);
            await _unitOfWork.CompleteAsync();
            
            return _mapper.Map<PropertyDTO>(property);
        }

        public async Task<PropertyDTO> UpdatePropertyAsync(PropertyDTO propertyDTO)
        {
            var existingProperty = await _unitOfWork.Properties.GetByIdAsync(propertyDTO.Id);
            if (existingProperty == null)
                return null;

            _mapper.Map(propertyDTO, existingProperty);
            existingProperty.ModifiedAt = DateTime.Now;
            
            _unitOfWork.Properties.Update(existingProperty);
            await _unitOfWork.CompleteAsync();
            
            return _mapper.Map<PropertyDTO>(existingProperty);
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            await _unitOfWork.Properties.DeleteAsync(id);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<bool> TogglePropertyStatusAsync(int id)
        {
            var property = await _unitOfWork.Properties.GetByIdAsync(id);
            if (property == null)
                return false;

            property.IsActive = !property.IsActive;
            property.ModifiedAt = DateTime.Now;
            
            _unitOfWork.Properties.Update(property);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
