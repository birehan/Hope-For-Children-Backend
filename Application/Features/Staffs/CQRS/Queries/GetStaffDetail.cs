

using MediatR;
using Application.Features.Staffs.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;

namespace Application.Features.Staffs.CQRS.Queries
{
    public class GetStaffDetailQuery : IRequest<Result<StaffDto>>
    {
        public Guid Id { get; set; }
    }

    public class GetStaffDetailQueryHandler : IRequestHandler<GetStaffDetailQuery, Result<StaffDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetStaffDetailQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<StaffDto>> Handle(GetStaffDetailQuery request, CancellationToken cancellationToken)
        {
            var Staff = await _unitOfWork.StaffRepository.Get(request.Id);
            return Result<StaffDto>.Success(_mapper.Map<StaffDto>(Staff));
        }
    }

}