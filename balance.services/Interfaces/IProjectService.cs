using balance.services.DTOs;

namespace balance.services.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync();
        Task<ProjectDTO> GetProjectByIdAsync(int id);
        Task<IEnumerable<ProjectDTO>> GetActiveProjectsAsync();
        Task<IEnumerable<ProjectDTO>> GetProjectsByAgentAsync(string agentId);
        Task<ProjectDTO> CreateProjectAsync(ProjectDTO projectDTO);
        Task<ProjectDTO> UpdateProjectAsync(ProjectDTO projectDTO);
        Task<bool> DeleteProjectAsync(int id);
        Task<bool> ToggleProjectStatusAsync(int id);
    }
}
