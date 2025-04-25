using balance.domain;
using balance.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.Repositories.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetActiveProjectsAsync()
        {
            return await _dbSet
                .Where(p => p.IsActive && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetByAgentAsync(string agentId)
        {
            return await _dbSet
                .Where(p => p.AgentId == agentId && !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetWithLocationAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .Include(p => p.Location)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetWithPropertiesAsync()
        {
            return await _dbSet
                .Where(p => !p.IsDeleted)
                .Include(p => p.Properties.Where(prop => !prop.IsDeleted))
                .ToListAsync();
        }
    }
}
