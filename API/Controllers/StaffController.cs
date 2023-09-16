using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Staffs.CQRS.Commands;
using Application.Features.Staffs.CQRS.Queries;
using Application.Features.Staffs.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace StaffsManagement.API.Controllers
{
    public class StaffsController : BaseApiController
    {
        private readonly IMediator _mediator;

        public StaffsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<List<StaffDto>>> Get()
        {
            return HandleResult(await _mediator.Send(new GetStaffListQuery()));
        }

        [AllowAnonymous]
        [HttpGet("sector")]
        public async Task<ActionResult<List<StaffDto>>> GetSectorStaffs(string sector)
        {
            return HandleResult(await _mediator.Send(new GetStaffOfSectorListQuery { Sector = sector }));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var command = new GetStaffDetailQuery { Id = id };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateStaffDto createTask)
        {

            var command = new CreateStaffCommand { StaffDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromForm] UpdateStaffDto updateStaffDto, Guid id)
        {
            updateStaffDto.Id = id;
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