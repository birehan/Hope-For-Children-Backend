using Application.Contracts.Persistence;
using Application.Features.SubCategories.DTOs;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.SubCategories.CQRS.Queries
{
    public class GetAllSubCategoryQuery : IRequest<Result<List<SubCategoryDto>>>
    {
    }

    public class GetAllSubCategoryQueryHandler : IRequestHandler<GetAllSubCategoryQuery, Result<List<SubCategoryDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoAccessor _photoAccessor;

        public GetAllSubCategoryQueryHandler(IMapper mapper, IUnitOfWork unitOfWork, IPhotoAccessor photoAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<List<SubCategoryDto>>> Handle(GetAllSubCategoryQuery request, CancellationToken cancellationToken)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetAllSubCategoryWithPhotos();

            var result = _mapper.Map<List<SubCategoryDto>>(subCategory);

            return Result<List<SubCategoryDto>>.Success(result);
        }
    }
}
