using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Projects.CQRS.Commands;
using Application.Features.Projects.CQRS.Queries;
using Application.Features.Projects.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ProjectsManagement.API.Controllers
{
    public class ProjectsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<ProjectDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetProjectListQuery()));
        }



        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var command = new GetProjectDetailQuery { Id = id };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateProjectDto createTask)
        {

            var command = new CreateProjectCommand { ProjectDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromForm] UpdateProjectDto updateProjectDto, Guid id)
        {
            updateProjectDto.Id = id;
            var command = new UpdateProjectCommand { ProjectDto = updateProjectDto };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteProjectCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }




    }
}