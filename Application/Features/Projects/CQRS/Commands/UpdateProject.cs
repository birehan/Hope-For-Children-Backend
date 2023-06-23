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

        private readonly IFileAccessor _fileAccessor;


        public UpdateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor, IFileAccessor fileAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
            _fileAccessor = fileAccessor;
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

                if (request.ProjectDto.ImageFile != null)
                {
                    var photoUploadResult = await _photoAccessor.UpdatePhoto(request.ProjectDto.ImageFile, Project.PhotoId);

                    if (photoUploadResult == null)
                        return Result<ProjectDto>.Failure("Creation Failed");

                    Project.Photo = new Photo
                    {
                        Url = photoUploadResult.Url,
                        Id = photoUploadResult.PublicId
                    };
                    Project.PhotoId = photoUploadResult.PublicId;
                }


                if (request.ProjectDto.PdfFile != null)
                {
                    var fileUploadResult = await _fileAccessor.UpdateFile(request.ProjectDto.PdfFile, Project.ProjectFileId);

                    if (fileUploadResult == null)
                        return Result<ProjectDto>.Failure("Creation Failed");

                    Project.ProjectFile = new ProjectFile
                    {
                        Url = fileUploadResult.Url,
                        Id = fileUploadResult.PublicId
                    };
                    Project.ProjectFileId = fileUploadResult.PublicId;
                }

                _unitOfWork.ProjectRepository.Update(Project);

                if (await _unitOfWork.Save() > 0)
                    return Result<ProjectDto>.Success(_mapper.Map<ProjectDto>(Project));

                return Result<ProjectDto>.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result<ProjectDto>.Failure($"Update failed: {ex.Message}");
            }
        }
    }
}