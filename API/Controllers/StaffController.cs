using API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        // Console.WriteLine("I am here Post Create staff API");

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] CreateStaffDto createTask)
        {

            var command = new CreateStaffCommand { StaffDto = createTask };
            return HandleResult(await _mediator.Send(command));
        }

        // [HttpPut("update-photo")]
        // public async Task<IActionResult> Put([FromForm] Guid Id, IFormFile File)
        // {

        //     var command = new UpdateStaffPhotoCommand { Id = Id, File = File };
        //     return HandleResult(await _mediator.Send(command));
        // }

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


        // [Authorize(Policy = "IsTaskCreator")]
        // [HttpPut("{id}")]
        // public async Task<IActionResult> Put([FromBody] UpdateTaskDto taskDto, int id)
        // {
        //     taskDto.Id = id;
        //     var command = new UpdateTaskCommand { TaskDto = taskDto };
        //     return HandleResult(await _mediator.Send(command));
        // }

        // [Authorize(Policy = "IsTaskCreator")]
        // [HttpDelete("{id}")]
        // public async Task<IActionResult> Delete(int id)
        // {
        //     var command = new DeleteTaskCommand { Id = id };
        //     return HandleResult(await _mediator.Send(command));
        // }

        // [HttpGet("{id}")]
        // public async Task<IActionResult> Get(int id)
        // {
        //     return HandleResult(await _mediator.Send(new GetTaskDetailQuery { Id = id }));
        // }

    }
}