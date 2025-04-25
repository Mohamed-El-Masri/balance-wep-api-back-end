using AutoMapper;
using balance.domain;
using balance.Repositories.Interfaces;
using balance.services.DTOs;
using balance.services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace balance.services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProjectDTO>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetAll(includeProperties: "Location,Properties,Agent")
                .Where(p => !p.IsDeleted)
                .ToListAsync();
                
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO> GetProjectByIdAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetAll(
                    p => p.Id == id,
                    null,
                    "Location,Properties,Agent")
                .FirstOrDefaultAsync();

            if (project == null)
                return null;

            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task<IEnumerable<ProjectDTO>> GetActiveProjectsAsync()
        {
            var projects = await _unitOfWork.Projects.GetActiveProjectsAsync();
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public async Task<IEnumerable<ProjectDTO>> GetProjectsByAgentAsync(string agentId)
        {
            var projects = await _unitOfWork.Projects.GetByAgentAsync(agentId);
            return _mapper.Map<IEnumerable<ProjectDTO>>(projects);
        }

        public async Task<ProjectDTO> CreateProjectAsync(ProjectDTO projectDTO)
        {
            var project = _mapper.Map<Project>(projectDTO);
            project.CreatedAt = DateTime.Now;
            
            await _unitOfWork.Projects.AddAsync(project);
            await _unitOfWork.CompleteAsync();
            
            return _mapper.Map<ProjectDTO>(project);
        }

        public async Task<ProjectDTO> UpdateProjectAsync(ProjectDTO projectDTO)
        {
            var existingProject = await _unitOfWork.Projects.GetByIdAsync(projectDTO.Id);
            if (existingProject == null)
                return null;

            _mapper.Map(projectDTO, existingProject);
            existingProject.ModifiedAt = DateTime.Now;
            
            _unitOfWork.Projects.Update(existingProject);
            await _unitOfWork.CompleteAsync();
            
            return _mapper.Map<ProjectDTO>(existingProject);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            await _unitOfWork.Projects.DeleteAsync(id);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }

        public async Task<bool> ToggleProjectStatusAsync(int id)
        {
            var project = await _unitOfWork.Projects.GetByIdAsync(id);
            if (project == null)
                return false;

            project.IsActive = !project.IsActive;
            project.ModifiedAt = DateTime.Now;
            
            _unitOfWork.Projects.Update(project);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;
        }
    }
}
