using MediatR;
using Application.Responses;
using Application.Contracts.Persistence;

namespace Application.Features.Staffs.CQRS.Commands
{
    public class DeleteStaffCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteStaffCommandHandler : IRequestHandler<DeleteStaffCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteStaffCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteStaffCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var staff = await _unitOfWork.StaffRepository.Get(request.Id);

                if (staff == null)
                    return Result<Guid>.Failure("Staff not found");

                _ = _unitOfWork.StaffRepository.Delete(staff);

                if (await _unitOfWork.Save() > 0)
                    return Result<Guid>.Success(staff.Id);

                return Result<Guid>.Failure("Deletion failed");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Deletion failed: {ex.Message}");
            }
        }
    }

}
