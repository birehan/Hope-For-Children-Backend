using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Staffs.CQRS.Commands;
using Application.Features.Staffs.CQRS.Queries;
using Application.Features.Staffs.DTOs;

namespace StaffsManagement.API.Controllers
{
    public class StaffsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public StaffsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<StaffDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetStaffListQuery()));
        }

        [HttpGet("sector")]
        public async Task<ActionResult<List<StaffDto>>> GetSectorStaffs(string sector)
        {
            return HandleResult(await _mediator.Send(new GetStaffOfSectorListQuery { Sector = sector }));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateStaffDto createTask)
        {

            var command = new CreateStaffCommand { StaffDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromForm] UpdateStaffDto updateStaffDto)
        {

            var command = new UpdateStaffCommand { StaffDto = updateStaffDto };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteStaffCommand { Id = id };
            return HandleResult(await _mediator.Send(command));
        }
    }
}