using DevFreela.API.Models;
using DevFreela.Application.Commands.CreateComment;
using DevFreela.Application.Commands.CreateProject;
using DevFreela.Application.Commands.DeleteProject;
using DevFreela.Application.Commands.UpdateProject;
using DevFreela.Application.InputModels;
using DevFreela.Application.Queries.GetAllProjects;
using DevFreela.Application.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DevFreela.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IProjectService _projectSerivce;
        private readonly IMediator _mediator;

        public ProjectsController(IProjectService projectSerivce, IMediator mediator)
        {
            _projectSerivce = projectSerivce;
            _mediator = mediator;
        }

        //private readonly ExampleClass _example;


        [HttpGet]
        [Authorize(Roles = "client, freelancer")]
        public async Task<IActionResult> Get(string queryParam)
        {
            var query = new GetAllProjectsQuery(queryParam);

            var projects = await _mediator.Send(query);

            return Ok(projects);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "client, freelancer")]
        public IActionResult GetById(int id)
        {
            var project = _projectSerivce.GetById(id);

            if (project == null) return NotFound();

            return Ok(project);
        }

        [HttpPost]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Post([FromBody] CreateProjectCommand command)
        {
            if (command.Title?.Length > 50)
            {
                return BadRequest();
            }

            var id = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetById), new { id }, command);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            if (command.Description?.Length > 200)
            {
                return BadRequest();
            }

            await _mediator.Send(command, cancellationToken);

            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteProjectCommand(id);

            await _mediator.Send(command, cancellationToken);
                 
            return NoContent();
        }

        [HttpPost("{id}/comments")]
        [Authorize(Roles = "client, freelancer")]
        public IActionResult PostComment(int id, [FromBody] CreateCommentCommand inputModel)
        {
            _mediator.Send(inputModel);

            return NoContent();
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}/start")]
        [Authorize(Roles = "client")]
        public IActionResult Start(int id)
        {
            _projectSerivce.Start(id);

            return NoContent();
        }

        [HttpPut("{id}/finish")]
        [Authorize(Roles = "client")]
        public void Finish(int id)
        {
            _projectSerivce.Finish(id);
            NoContent();
        }

    }
}
