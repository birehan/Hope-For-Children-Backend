
using MediatR;
using Application.Features.Staffs.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;

namespace Application.Features.Staffs.CQRS.Queries
{
    public class GetStaffListQuery : IRequest<Result<List<StaffDto>>>

    {

    }

    public class GetStaffListQueryHandler : IRequestHandler<GetStaffListQuery, Result<List<StaffDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStaffListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<StaffDto>>> Handle(GetStaffListQuery request, CancellationToken cancellationToken)
        {
            var Staffs = await _unitOfWork.StaffRepository.GetAll();
            return Result<List<StaffDto>>.Success(_mapper.Map<List<StaffDto>>(Staffs));
        }
    }

}