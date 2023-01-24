using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Queries;

public class GetAllProjectsQueryHandlerTests
{
    [Fact]
    public async Task ThreeProjectsExists_Executed_ReturnThreeProjectsViewModels()
    {
        var projects = new List<Project>
        {
            new Project("Nome Do Teste 1", "Description do test 1", 1, 2, 3000),
            new Project("Nome Do Teste 2", "Description do test 2", 1, 3, 3000),
            new Project("Nome Do Teste 3", "Description do test 3", 1, 4, 3000)
        };

        var projectRepositoryMock = new Mock<IProjectRepository>();
        projectRepositoryMock.Setup(x => x.GetAllAsync().Result).Returns(projects);

        var getAllProjectsQuery = new GetAllProjectsQuery("");
        var getAllProjectsQueryHandler = new GetAllProjectsQueryHandler(projectRepositoryMock.Object);
        
        var projectViewModelList = await getAllProjectsQueryHandler.Handle(getAllProjectsQuery, new CancellationToken());

        Assert.NotNull(projectViewModelList);
        Assert.NotEmpty(projectViewModelList);
        Assert.Equal(projects.Count, projectViewModelList.Count);
        
        projectRepositoryMock.Verify(pr => pr.GetAllAsync().Result, Times.Once);

    }
}