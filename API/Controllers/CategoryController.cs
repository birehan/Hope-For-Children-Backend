using Application.Features.Categories.CQRS.Commands;
using Application.Features.Categories.CQRS.Queries;
using Application.Features.Categories.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] GalleryDto GalleryDto)
        {
            return HandleResult(await _mediator.Send(new CreateCategoryCommand { GalleryDto = GalleryDto }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory([FromForm] UpdateGalleryDto GalleryDto, Guid id)
        {
            GalleryDto.Id = id;
            return HandleResult(await _mediator.Send(new UpdateCategoryCommand { GalleryDto = GalleryDto }));
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return HandleResult(await _mediator.Send(new GetCategoryByIdQuery { Id = Id }));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            return HandleResult(await _mediator.Send(new GetAllCategoryQuery()));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteCategory(Guid Id)
        {
            return HandleResult(await _mediator.Send(new DeleteCategoryCommand { Id = Id }));
        }
    }
}
