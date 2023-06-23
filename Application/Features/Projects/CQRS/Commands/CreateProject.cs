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
    public class CreateProjectCommand : IRequest<Result<ProjectDto>>
    {
        public CreateProjectDto ProjectDto { get; set; }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, Result<ProjectDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IPhotoAccessor _photoAccessor;


        private readonly IFileAccessor _fileAccessor;



        public CreateProjectCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor, IFileAccessor fileAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
            _fileAccessor = fileAccessor;
        }

        public async Task<Result<ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {

            try
            {

                var validator = new CreateProjectDtoValidator();
                var validationResult = await validator.ValidateAsync(request.ProjectDto);

                if (!validationResult.IsValid)
                    return Result<ProjectDto>.Failure(validationResult.Errors[0].ErrorMessage);


                var Project = _mapper.Map<Project>(request.ProjectDto);
                var photoUploadResult = await _photoAccessor.AddPhoto(request.ProjectDto.ImageFile);

                if (photoUploadResult == null)
                    return Result<ProjectDto>.Failure("Creation Failed due to photo upload error.");

                Project.Photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId,
                };
                Project.PhotoId = photoUploadResult.PublicId;


                var fileUploadResult = await _fileAccessor.UploadFile(request.ProjectDto.PdfFile);

                if (fileUploadResult == null)
                    return Result<ProjectDto>.Failure("Creation Failed due to file upload error.");

                Project.ProjectFile = new ProjectFile
                {
                    Url = fileUploadResult.Url,
                    Id = fileUploadResult.PublicId,
                };
                Project.ProjectFileId = fileUploadResult.PublicId;

                await _unitOfWork.ProjectRepository.Add(Project);

                if (await _unitOfWork.Save() > 0)
                    return Result<ProjectDto>.Success(_mapper.Map<ProjectDto>(Project));

                return Result<ProjectDto>.Failure("Creation Failed");
            }
            catch (Exception ex)
            {
                return Result<ProjectDto>.Failure($"Creation Failed: {ex.Message}");

            }
        }
    }

}
