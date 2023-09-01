using Application.Contracts.Persistence;
using Application.Features.Categories.DTOs;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Categories.CQRS.Queries
{
    public class GetCategoryByIdQuery : IRequest<Result<CategoryDetailDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryDetailDto>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetCategoryByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CategoryDetailDto>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            var Category = await _unitOfWork.CategoryRepository.GetCategoryWithPhotos(request.Id);

            var result = _mapper.Map<CategoryDetailDto>(Category);

            return Result<CategoryDetailDto>.Success(result);
        }
    }
}
