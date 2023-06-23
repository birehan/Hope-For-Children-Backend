using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Alumnis.CQRS.Commands;
using Application.Features.Alumnis.CQRS.Queries;
using Application.Features.Alumnis.DTOs;

namespace AlumnisManagement.API.Controllers
{
    public class AlumnisController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AlumnisController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<AlumniDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetAlumniListQuery()));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateAlumniDto createTask)
        {

            var command = new CreateAlumniCommand { AlumniDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UpdateAlumniDto updateAlumniDto)
        {

            var command = new UpdateAlumniCommand { AlumniDto = updateAlumniDto };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteAlumniCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }




    }
}