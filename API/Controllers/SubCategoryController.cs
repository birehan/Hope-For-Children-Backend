using Application.Features.Categories.Commands;
using Application.Features.SubCategories.CQRS.Commands;
using Application.Features.SubCategories.CQRS.Queries;
using Application.Features.SubCategories.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : BaseApiController
    {
        private readonly IMediator _mediator;

        public SubCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateSubCategory([FromForm]CreateSubCategoryDto SubCategoryDto)
        {
            return HandleResult(await _mediator.Send(new CreateSubCategoryCommand {SubCategoryDto = SubCategoryDto }));
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateSubCategory([FromForm]UpdateSubCategoryDto subCategoryDto)
        {
            return HandleResult(await _mediator.Send(new UpdateSubCategoryCommand { SubCategoryDto = subCategoryDto}));
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return HandleResult(await _mediator.Send(new GetSubCategoryByIdQuery { Id = Id }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await _mediator.Send(new GetAllSubCategoryQuery()));
        }

        [HttpDelete]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteSubCategory(Guid Id)
        {
            return HandleResult(await _mediator.Send(new DeleteSubCategoryCommand { Id = Id}));
        }
    }
}
