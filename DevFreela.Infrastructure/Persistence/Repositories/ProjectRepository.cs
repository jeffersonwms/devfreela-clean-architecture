using Dapper;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly DevFreelaDbContext _dbContext;
    private readonly string _connString;


    public ProjectRepository(DevFreelaDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _connString = configuration.GetConnectionString("DevFreelaCs");
    }

    public async  Task<List<Project>> GetAllAsync()
    {
        return await _dbContext.Projects.ToListAsync();
    }

    public async Task<Project> GetByIdAsync(int projectId)
    {
        return (await _dbContext.Projects
            .Include(p => p.Client)
            .Include(p => p.Freelancer)
            .SingleOrDefaultAsync(x => x.Id == projectId))!;

    }

    public async Task AddAsync(Project project, CancellationToken cancellationToken)
    {
        await _dbContext.Projects.AddAsync(project, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }


    // public async Task DeleteAsync(int projectId, CancellationToken cancellationToken)
    // {
    //     throw new NotImplementedException();
    // }

    public async Task StartAsync(Project project, CancellationToken cancellationToken)
    {
        await using var sqlConnection = new SqlConnection(_connString);
        await sqlConnection.OpenAsync(cancellationToken);

        var script = "UPDATE Projects SET Status = @status, StartedAt = @startedAt WHERE Id = @projectId";
            
        await sqlConnection.ExecuteAsync(script, new { status = project?.Status, StartedAt = project?.StartedAt, Id = project?.Id });
    }
    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}