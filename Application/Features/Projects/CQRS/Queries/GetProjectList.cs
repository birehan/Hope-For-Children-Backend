
using MediatR;
using Application.Features.Projects.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;

namespace Application.Features.Projects.CQRS.Queries
{
    public class GetProjectListQuery : IRequest<Result<List<ProjectDto>>>
    { }

    public class GetProjectListQueryHandler : IRequestHandler<GetProjectListQuery, Result<List<ProjectDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProjectListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<ProjectDto>>> Handle(GetProjectListQuery request, CancellationToken cancellationToken)
        {
            var Projects = await _unitOfWork.ProjectRepository.GetAll();
            return Result<List<ProjectDto>>.Success(_mapper.Map<List<ProjectDto>>(Projects));
        }
    }

}