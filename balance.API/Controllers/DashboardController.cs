using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Agent")]
    public class DashboardController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public DashboardController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("agent-properties")]
        [Authorize(Roles = "Agent")]
        public async Task<ActionResult<IEnumerable<PropertyDTO>>> GetAgentProperties()
        {
            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var properties = await _serviceManager.PropertyService.GetPropertiesByAgentAsync(agentId);
            return Ok(properties);
        }

        [HttpGet("agent-projects")]
        [Authorize(Roles = "Agent")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAgentProjects()
        {
            var agentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var projects = await _serviceManager.ProjectService.GetProjectsByAgentAsync(agentId);
            return Ok(projects);
        }

        [HttpGet("agents")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllAgents()
        {
            var agents = await _serviceManager.UserProfileService.GetUsersByRoleAsync("Agent");
            return Ok(agents);
        }

        [HttpGet("customers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllCustomers()
        {
            var customers = await _serviceManager.UserProfileService.GetUsersByRoleAsync("Customer");
            return Ok(customers);
        }

        [HttpGet("property-types-stats")]
        public async Task<ActionResult> GetPropertyTypesStats()
        {
            var propertyTypes = await _serviceManager.PropertyTypeService.GetPropertyTypesWithCountsAsync();
            return Ok(propertyTypes);
        }
    }
}
