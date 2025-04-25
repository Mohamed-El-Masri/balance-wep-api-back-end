using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace balance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public ProjectController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetAllProjects()
        {
            var projects = await _serviceManager.ProjectService.GetAllProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDTO>> GetProjectById(int id)
        {
            var project = await _serviceManager.ProjectService.GetProjectByIdAsync(id);
            
            if (project == null)
                return NotFound();
                
            return Ok(project);
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetActiveProjects()
        {
            var projects = await _serviceManager.ProjectService.GetActiveProjectsAsync();
            return Ok(projects);
        }

        [HttpGet("agent/{agentId}")]
        public async Task<ActionResult<IEnumerable<ProjectDTO>>> GetProjectsByAgent(string agentId)
        {
            var projects = await _serviceManager.ProjectService.GetProjectsByAgentAsync(agentId);
            return Ok(projects);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult<ProjectDTO>> CreateProject([FromBody] ProjectDTO projectDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Set the agent ID to the current user if they're an agent
            if (User.IsInRole("Agent"))
            {
                projectDTO.AgentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }

            var project = await _serviceManager.ProjectService.CreateProjectAsync(projectDTO);
            
            return CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, project);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult<ProjectDTO>> UpdateProject(int id, [FromBody] ProjectDTO projectDTO)
        {
            if (id != projectDTO.Id)
                return BadRequest("Project ID mismatch");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verify ownership if the user is an agent
            if (User.IsInRole("Agent"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var project = await _serviceManager.ProjectService.GetProjectByIdAsync(id);
                
                if (project == null)
                    return NotFound();
                    
                if (project.AgentId != currentUserId)
                    return Forbid();
            }

            var updatedProject = await _serviceManager.ProjectService.UpdateProjectAsync(projectDTO);
            
            if (updatedProject == null)
                return NotFound();
                
            return Ok(updatedProject);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult> DeleteProject(int id)
        {
            // Verify ownership if the user is an agent
            if (User.IsInRole("Agent"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var project = await _serviceManager.ProjectService.GetProjectByIdAsync(id);
                
                if (project == null)
                    return NotFound();
                    
                if (project.AgentId != currentUserId)
                    return Forbid();
            }

            var result = await _serviceManager.ProjectService.DeleteProjectAsync(id);
            
            if (!result)
                return NotFound();
                
            return NoContent();
        }

        [HttpPut("toggle-status/{id}")]
        [Authorize(Roles = "Admin,Agent")]
        public async Task<ActionResult> ToggleProjectStatus(int id)
        {
            // Verify ownership if the user is an agent
            if (User.IsInRole("Agent"))
            {
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var project = await _serviceManager.ProjectService.GetProjectByIdAsync(id);
                
                if (project == null)
                    return NotFound();
                    
                if (project.AgentId != currentUserId)
                    return Forbid();
            }

            var result = await _serviceManager.ProjectService.ToggleProjectStatusAsync(id);
            
            if (!result)
                return NotFound();
                
            return Ok();
        }
    }
}
