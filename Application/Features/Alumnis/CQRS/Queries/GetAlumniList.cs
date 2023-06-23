
using MediatR;
using Application.Features.Alumnis.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;

namespace Application.Features.Alumnis.CQRS.Queries
{
    public class GetAlumniListQuery : IRequest<Result<List<AlumniDto>>>
    { }

    public class GetAlumniListQueryHandler : IRequestHandler<GetAlumniListQuery, Result<List<AlumniDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAlumniListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<AlumniDto>>> Handle(GetAlumniListQuery request, CancellationToken cancellationToken)
        {
            var Alumnis = await _unitOfWork.AlumniRepository.GetAll();
            return Result<List<AlumniDto>>.Success(_mapper.Map<List<AlumniDto>>(Alumnis));
        }
    }

}