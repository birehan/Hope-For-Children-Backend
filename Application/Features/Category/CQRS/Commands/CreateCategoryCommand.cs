using Application.Contracts.Persistence;
using Application.Features.Categories.DTOs;
using Application.Features.SubCategories.CQRS.Commands;
using Application.Features.SubCategories.DTOs;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using Domain;
using MediatR;

namespace Application.Features.Categories.Commands
{
    public class CreateCategoryCommand : IRequest<Result<Guid>>
    {
        public CreateCategoryDto CategoryDto { get; set; }
    }

    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;
        private readonly IMediator _mediator;

        public CreateCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
            _mediator = mediator;
        }


        public async Task<Result<Guid>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var existingCategory = await _unitOfWork.CategoryRepository.GetByTitleAsync(request.CategoryDto.Title);
            var categoryId = new Guid();

            if (existingCategory != null)
            {
                categoryId = existingCategory.Id;
            }

            else
            {
                var category = _mapper.Map<Category>(request.CategoryDto);
                await _unitOfWork.CategoryRepository.Add(category);
                categoryId = category.Id;
            }


            foreach (var subCategoryDto in request.CategoryDto.SubCategories)
            {
                var createSubCategoryDto = new CreateSubCategoryDto
                {
                    Title = subCategoryDto.Title,
                    CategoryId = categoryId,
                    Photos = subCategoryDto.Photos,
                    MainPhoto = subCategoryDto.MainPhoto
                };

                var createSubCategoryCommand = new CreateSubCategoryCommand { SubCategoryDto = createSubCategoryDto };

                var result = await _mediator.Send(createSubCategoryCommand, cancellationToken);

                if (!result.IsSuccess)
                {
                    return Result<Guid>.Failure(result.Error);
                }
            }

            if (await _unitOfWork.Save() > 0)
                return Result<Guid>.Success(categoryId);

            return Result<Guid>.Failure("Error while saving chages");
        }
    }
}
