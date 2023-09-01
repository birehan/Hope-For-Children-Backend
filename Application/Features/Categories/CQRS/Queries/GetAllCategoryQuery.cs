using Application.Contracts.Persistence;
using Application.Features.Categories.DTOs;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Categories.CQRS.Queries
{
    public class GetAllCategoryQuery : IRequest<Result<List<CategoryDto>>>
    {
    }

    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, Result<List<CategoryDto>>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public GetAllCategoryQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<CategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var Category = await _unitOfWork.CategoryRepository.GetAll();

            var result = _mapper.Map<List<CategoryDto>>(Category);

            return Result<List<CategoryDto>>.Success(result);
        }
    }
}
