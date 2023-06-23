using MediatR;
using Application.Responses;
using Application.Contracts.Persistence;

namespace Application.Features.Projects.CQRS.Commands
{
    public class DeleteProjectCommand : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, Result<Guid>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var Project = await _unitOfWork.ProjectRepository.Get(request.Id);

                if (Project == null)
                    return Result<Guid>.Failure("Project not found");

                _ = _unitOfWork.ProjectRepository.Delete(Project);

                if (await _unitOfWork.Save() > 0)
                    return Result<Guid>.Success(Project.Id);

                return Result<Guid>.Failure("Deletion failed");
            }
            catch (Exception ex)
            {
                return Result<Guid>.Failure($"Deletion failed: {ex.Message}");
            }
        }
    }

}
