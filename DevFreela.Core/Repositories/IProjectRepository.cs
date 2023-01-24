using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevFreela.Core.Entities;

namespace DevFreela.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<List<Project>> GetAllAsync();
        Task<Project> GetByIdAsync(int projectId);
        Task AddAsync(Project project, CancellationToken cancellationToken);
        // Task DeleteAsync(int projectId, CancellationToken cancellationToken);
        Task StartAsync(Project project, CancellationToken cancellationToken);
        Task SaveChangesAsync();
    }
}
