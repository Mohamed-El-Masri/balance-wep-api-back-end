using balance.domain.Enums;
using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PropertyController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAllProperties()
        {
            var properties = await _serviceManager.PropertyService.GetAllPropertiesAsync();
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyDTO>> GetPropertyById(int id)
        {
            var property = await _serviceManager.PropertyService.GetPropertyByIdAsync(id);
            
            if (property == null)
                return NotFound();
                
            return Ok(property);
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByStatus(Status status)
        {
            var properties = await _serviceManager.PropertyService.GetPropertiesByStatusAsync(status);
            return Ok(properties);
        }

        [HttpGet("type/{typeId}")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByType(int typeId)
        {
            var properties = await _serviceManager.PropertyService.GetPropertiesByTypeAsync(typeId);
            return Ok(properties);
        }

        [HttpGet("price-range")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var properties = await _serviceManager.PropertyService.GetPropertiesByPriceRangeAsync(minPrice, maxPrice);
            return Ok(properties);
        }

        [HttpGet("agent/{agentId}")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByAgent(string agentId)
        {
            var properties = await _serviceManager.PropertyService.GetPropertiesByAgentAsync(agentId);
            return Ok(properties);
        }

        [HttpGet("project/{projectId}")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByProject(int projectId)
        {
            var properties = await _serviceManager.PropertyService.GetPropertiesByProjectAsync(projectId);
            return Ok(properties);
        }

        [HttpGet("offer-type/{offerType}")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetPropertiesByOfferType(offer_type offerType)
        {
            var properties = await _serviceManager.PropertyService.GetPropertiesByOfferTypeAsync(offerType);
            return Ok(properties);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult<PropertyDTO>> CreateProperty([FromBody] PropertyDTO propertyDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Set the agent ID to the current user if they're an agent
            if (User.IsInRole("Agent"))
            {
                propertyDTO.AgentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var property = await _serviceManager.PropertyService.CreatePropertyAsync(propertyDTO);
            
            return CreatedAtAction(nameof(GetPropertyById), new { id = property.Id }, property);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult<PropertyDTO>> UpdateProperty(int id, [FromBody] PropertyDTO propertyDTO)
        {
            if (id != propertyDTO.Id)
                return BadRequest("Property ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verify ownership if the user is an agent
            if (User.IsInRole("Agent"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var property = await _serviceManager.PropertyService.GetPropertyByIdAsync(id);
                
                if (property == null)
                    return NotFound();
                    
                if (property.AgentId != currentUserId)
                    return Forbid();
            }

            var updatedProperty = await _serviceManager.PropertyService.UpdatePropertyAsync(propertyDTO);
            
            if (updatedProperty == null)
                return NotFound();
                
            return Ok(updatedProperty);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult> DeleteProperty(int id)
        {
            // Verify ownership if the user is an agent
            if (User.IsInRole("Agent"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var property = await _serviceManager.PropertyService.GetPropertyByIdAsync(id);
                
                if (property == null)
                    return NotFound();
                    
                if (property.AgentId != currentUserId)
                    return Forbid();
            }

            var result = await _serviceManager.PropertyService.DeletePropertyAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }

        [HttpPut("toggle-status/{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult> TogglePropertyStatus(int id)
        {
            // Verify ownership if the user is an agent
            if (User.IsInRole("Agent"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var property = await _serviceManager.PropertyService.GetPropertyByIdAsync(id);
                
                if (property == null)
                    return NotFound();
                    
                if (property.AgentId != currentUserId)
                    return Forbid();
            }

            var result = await _serviceManager.PropertyService.TogglePropertyStatusAsync(id);
            
            if (!result)
                return NotFound();
                
            return Ok();
        }
    }
}
