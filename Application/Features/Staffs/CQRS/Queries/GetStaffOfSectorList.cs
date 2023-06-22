
using MediatR;
using Application.Features.Staffs.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;
using static Domain.Staff;

namespace Application.Features.Staffs.CQRS.Queries
{
    public class GetStaffOfSectorListQuery : IRequest<Result<List<StaffDto>>>
    {
        public string Sector { get; set; }
    }

    public class GetStaffOfSectorListQueryHandler : IRequestHandler<GetStaffOfSectorListQuery, Result<List<StaffDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStaffOfSectorListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<StaffDto>>> Handle(GetStaffOfSectorListQuery request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse(request.Sector, out Sector sector))
            {
                return Result<List<StaffDto>>.Failure("Invalid sector");
            }

            var staffs = await _unitOfWork.StaffRepository.GetStaffsBySector(sector);
            var staffDtos = _mapper.Map<List<StaffDto>>(staffs);

            return Result<List<StaffDto>>.Success(staffDtos);
        }
    }
}