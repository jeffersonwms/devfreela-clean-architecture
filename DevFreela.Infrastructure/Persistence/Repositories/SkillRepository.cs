using Dapper;
using DevFreela.Core.DTOs;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DevFreela.Infrastructure.Persistence.Repositories;

public class SkillRepository : ISkillRepository
{
    private readonly DevFreelaDbContext _dbContext;
    private readonly string _connString;


    public SkillRepository(DevFreelaDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _connString = configuration.GetConnectionString("DevFreelaCs");
    }

    public async  Task<List<SkillDTO>> GetAllAsync()
    {
        await using var sqlConnection = new SqlConnection(_connString);
        sqlConnection.Open();

        const string script = "SELECT Id, Description FROM Skills";

        var skills = await sqlConnection.QueryAsync<SkillDTO>(script);

        return skills.ToList();
    }
}