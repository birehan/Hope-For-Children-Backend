using MediatR;
using Application.Features.Projects.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;
using Application.Interfaces;
using HFC.Application.Features.Projects.DTOs.Validators;
using Domain;

namespace Application.Features.Projects.CQRS.Commands
{
    public class UpdateProjectCommand : IRequest<Result<ProjectDto>>
    {
        public UpdateProjectDto ProjectDto { get; set; }
    }

    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, Result<ProjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;



        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }


        public async Task<Result<ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var validator = new UpdateProjectDtoValidator();
                var validationResult = await validator.ValidateAsync(request.ProjectDto);

                if (!validationResult.IsValid)
                    return Result<ProjectDto>.Failure(validationResult.Errors[0].ErrorMessage);

                var Project = await _unitOfWork.ProjectRepository.Get(request.ProjectDto.Id);

                if (Project == null)
                    return Result<ProjectDto>.Failure("Project not found");

                Project.Title = request.ProjectDto.Title;
                Project.Description = request.ProjectDto.Description;

                if (request.ProjectDto.File != null)
                {
                    var photoUploadResult = await _photoAccessor.UpdatePhoto(request.ProjectDto.File, Project.PhotoId);

                    if (photoUploadResult == null)
                        return Result<ProjectDto>.Failure("Creation Failed");

                    Project.Photo = new Photo
                    {
                        Url = photoUploadResult.Url,
                        Id = photoUploadResult.PublicId
                    };
                    Project.PhotoId = photoUploadResult.PublicId;
                }
                var sample = _mapper.Map<ProjectDto>(Project);

                _unitOfWork.ProjectRepository.Update(Project);

                if (await _unitOfWork.Save() > 0)
                    return Result<ProjectDto>.Success(sample);

                return Result<ProjectDto>.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result<ProjectDto>.Failure($"Update failed: {ex.Message}");
            }
        }
    }
}
