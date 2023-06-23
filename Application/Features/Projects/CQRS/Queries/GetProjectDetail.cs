
using MediatR;
using Application.Features.Projects.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;

namespace Application.Features.Projects.CQRS.Queries
{
    public class GetProjectDetailQuery : IRequest<Result<ProjectDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetProjectDetailQueryHandler : IRequestHandler<GetProjectDetailQuery, Result<ProjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetProjectDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ProjectDto>> Handle(GetProjectDetailQuery request, CancellationToken cancellationToken)
        {
            var Projects = await _unitOfWork.ProjectRepository.Get(request.Id);
            return Result<ProjectDto>.Success(_mapper.Map<ProjectDto>(Projects));
        }
    }

}