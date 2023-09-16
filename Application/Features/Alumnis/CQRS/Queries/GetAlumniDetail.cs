
using MediatR;
using Application.Features.Alumnis.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;

namespace Application.Features.Alumnis.CQRS.Queries
{
    public class GetAlumniDetailQuery : IRequest<Result<AlumniDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetAlumniDetailQueryHandler : IRequestHandler<GetAlumniDetailQuery, Result<AlumniDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAlumniDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<AlumniDto>> Handle(GetAlumniDetailQuery request, CancellationToken cancellationToken)
        {
            var Staff = await _unitOfWork.AlumniRepository.Get(request.Id);
            return Result<AlumniDto>.Success(_mapper.Map<AlumniDto>(Staff));
        }
    }

}