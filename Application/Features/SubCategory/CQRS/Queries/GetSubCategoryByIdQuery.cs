using Application.Contracts.Persistence;
using Application.Features.SubCategories.DTOs;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.SubCategories.CQRS.Queries
{
    public class GetSubCategoryByIdQuery : IRequest<Result<SubCategoryDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetSubCategoryByIdQueryHandler : IRequestHandler<GetSubCategoryByIdQuery, Result<SubCategoryDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoAccessor _photoAccessor;

        public GetSubCategoryByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IPhotoAccessor photoAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<SubCategoryDto>> Handle(GetSubCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetSubCategoryWithPhotos(request.Id);

            var result = _mapper.Map<SubCategoryDto>(subCategory);

            return Result<SubCategoryDto>.Success(result);
        }
    }
}
