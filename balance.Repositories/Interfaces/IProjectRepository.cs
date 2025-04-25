using balance.domain;

namespace balance.Repositories.Interfaces
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        // Get projects by agent
        Task<IEnumerable<Project>> GetByAgentAsync(string agentId);
        
        // Get active projects
        Task<IEnumerable<Project>> GetActiveProjectsAsync();
        
        // Get projects with properties
        Task<IEnumerable<Project>> GetWithPropertiesAsync();
        
        // Get projects with location
        Task<IEnumerable<Project>> GetWithLocationAsync();
    }
}
