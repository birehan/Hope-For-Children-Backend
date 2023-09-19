using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Categories.DTOs.Validators
{
    public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryDtoValidator()
        {
            Include(new ICategoryDtoValidator());
        }

    }
}
