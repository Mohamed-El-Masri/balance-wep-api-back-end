using balance.domain.Enums;
using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public HomeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("featured-properties")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetFeaturedProperties()
        {
            // Get active properties with available status
            var properties = await _serviceManager.PropertyService.GetPropertiesByStatusAsync(Status.Available);
            
            // Take a few properties to showcase
            return Ok(properties.Take(6));
        }

        [HttpGet("latest-properties")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetLatestProperties()
        {
            var properties = await _serviceManager.PropertyService.GetAllPropertiesAsync();
            
            // Order by posted date and take recent ones
            return Ok(properties.OrderByDescending(p => p.PostedDate).Take(8));
        }

        [HttpGet("featured-projects")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetFeaturedProjects()
        {
            var projects = await _serviceManager.ProjectService.GetActiveProjectsAsync();
            
            // Take a few projects to showcase
            return Ok(projects.Take(4));
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> SearchProperties(
            [FromQuery] string? keyword,
            [FromQuery] int? typeId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] int? bedrooms,
            [FromQuery] Status? status,
            [FromQuery] offer_type? offerType)
        {
            // Get all properties
            var properties = await _serviceManager.PropertyService.GetAllPropertiesAsync();
            
            // Apply filters
            var filteredProperties = properties.AsEnumerable();
            
            if (!string.IsNullOrEmpty(keyword))
            {
                filteredProperties = filteredProperties.Where(p => 
                    p.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Address.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            }
            
            if (typeId.HasValue)
            {
                filteredProperties = filteredProperties.Where(p => p.PropertyTypeId == typeId.Value);
            }
            
            if (minPrice.HasValue)
            {
                filteredProperties = filteredProperties.Where(p => p.Price >= minPrice.Value);
            }
            
            if (maxPrice.HasValue)
            {
                filteredProperties = filteredProperties.Where(p => p.Price <= maxPrice.Value);
            }
            
            if (bedrooms.HasValue)
            {
                filteredProperties = filteredProperties.Where(p => p.Bedrooms >= bedrooms.Value);
            }
            
            if (status.HasValue)
            {
                filteredProperties = filteredProperties.Where(p => p.Status == status.Value);
            }
            
            if (offerType.HasValue)
            {
                filteredProperties = filteredProperties.Where(p => p.OfferType == offerType.Value);
            }
            
            return Ok(filteredProperties.ToList());
        }

        [HttpGet("property-types")]
        public async Task<ActionResult<IEnumerable<PropertyTypeDTO>>> GetPropertyTypes()
        {
            var propertyTypes = await _serviceManager.PropertyTypeService.GetAllPropertyTypesAsync();
            return Ok(propertyTypes);
        }
    }
}
