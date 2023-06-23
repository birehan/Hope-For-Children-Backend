using Application.Features.Categories.Commands;
using Application.Features.Categories.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseApiController
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateCategory([FromForm]CreateCategoryDto categoryDto)
        {
            return HandleResult(await _mediator.Send(new CreateCategoryCommand { CategoryDto = categoryDto}));
        }
    }
}
