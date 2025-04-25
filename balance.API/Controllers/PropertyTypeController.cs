using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyTypeController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public PropertyTypeController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PropertyTypeDTO>>> GetAllPropertyTypes()
        {
            var propertyTypes = await _serviceManager.PropertyTypeService.GetAllPropertyTypesAsync();
            return Ok(propertyTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PropertyTypeDTO>> GetPropertyTypeById(int id)
        {
            var propertyType = await _serviceManager.PropertyTypeService.GetPropertyTypeByIdAsync(id);
            
            if (propertyType == null)
                return NotFound();
                
            return Ok(propertyType);
        }

        [HttpGet("with-counts")]
        public async Task<ActionResult<IEnumerable<PropertyTypeDTO>>> GetPropertyTypesWithCounts()
        {
            var propertyTypes = await _serviceManager.PropertyTypeService.GetPropertyTypesWithCountsAsync();
            return Ok(propertyTypes);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PropertyTypeDTO>> CreatePropertyType([FromBody] PropertyTypeDTO propertyTypeDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var propertyType = await _serviceManager.PropertyTypeService.CreatePropertyTypeAsync(propertyTypeDTO);
            
            return CreatedAtAction(nameof(GetPropertyTypeById), new { id = propertyType.Id }, propertyType);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<PropertyTypeDTO>> UpdatePropertyType(int id, [FromBody] PropertyTypeDTO propertyTypeDTO)
        {
            if (id != propertyTypeDTO.Id)
                return BadRequest("PropertyType ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedPropertyType = await _serviceManager.PropertyTypeService.UpdatePropertyTypeAsync(propertyTypeDTO);
            
            if (updatedPropertyType == null)
                return NotFound();
                
            return Ok(updatedPropertyType);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeletePropertyType(int id)
        {
            var result = await _serviceManager.PropertyTypeService.DeletePropertyTypeAsync(id);
            
            if (!result)
                return BadRequest("Cannot delete property type that is in use by properties");
                
            return NoContent();
        }
    }
}
