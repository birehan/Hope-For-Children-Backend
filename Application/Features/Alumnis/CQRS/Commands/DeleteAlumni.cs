using MediatR;
using Application.Responses;
using Application.Contracts.Persistence;

namespace Application.Features.Alumnis.CQRS.Commands
{
    public class DeleteAlumniCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteAlumniCommandHandler : IRequestHandler<DeleteAlumniCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteAlumniCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteAlumniCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Alumni = await _unitOfWork.AlumniRepository.Get(request.Id);

                if (Alumni == null)
                    return Result<Guid>.Failure("Alumni not found");

                _ = _unitOfWork.AlumniRepository.Delete(Alumni);

                if (await _unitOfWork.Save() > 0)
                    return Result<Guid>.Success(Alumni.Id);

                return Result<Guid>.Failure("Deletion failed");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Deletion failed: {ex.Message}");
            }
        }
    }

}
