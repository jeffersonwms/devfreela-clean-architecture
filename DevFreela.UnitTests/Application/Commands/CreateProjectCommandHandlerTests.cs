using DevFreela.Application.Commands.CreateProject;
using DevFreela.Core.Entities;
using DevFreela.Core.Repositories;
using Moq;

namespace DevFreela.UnitTests.Application.Commands;

public class CreateProjectCommandHandlerTests
{
    [Fact]
    public async Task InputDataIsOk_Executed_ReturnsProjectId()
    {
        //Arrange
        var projectRepositoryMock = new Mock<IProjectRepository>();

        var createProjectCommand = new CreateProjectCommand
        {
            Title = "Titulo Teste",
            Description = "A nice description",
            IdClient = 1,
            IdFreelancer = 2,
            TotalCost = 3000
        };

        var createProjectCommandHandler = new CreateProjectCommandHandler(projectRepositoryMock.Object);

        //Act
        var id = await createProjectCommandHandler.Handle(createProjectCommand, new CancellationToken());
        
        //Assert
        Assert.True(id >= 0);
        
        projectRepositoryMock.Verify(pr => pr.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()), Times.Once);
        
    }
}