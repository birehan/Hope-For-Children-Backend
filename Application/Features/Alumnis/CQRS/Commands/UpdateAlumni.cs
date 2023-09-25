using MediatR;
using Application.Features.Alumnis.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;
using Application.Interfaces;
using HFC.Application.Features.Alumnis.DTOs.Validators;
using Domain;

namespace Application.Features.Alumnis.CQRS.Commands
{
    public class UpdateAlumniCommand : IRequest<Result<AlumniDto>>
    {
        public UpdateAlumniDto AlumniDto { get; set; }
    }

    public class UpdateAlumniCommandHandler : IRequestHandler<UpdateAlumniCommand, Result<AlumniDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;

        public UpdateAlumniCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<AlumniDto>> Handle(UpdateAlumniCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateAlumniDtoValidator();
                var validationResult = await validator.ValidateAsync(request.AlumniDto);

                if (!validationResult.IsValid)
                    return Result<AlumniDto>.Failure(validationResult.Errors[0].ErrorMessage);

                var Alumni = await _unitOfWork.AlumniRepository.Get(request.AlumniDto.Id);

                if (Alumni == null)
                    return Result<AlumniDto>.Failure("Alumni not found");

                Alumni.Name = request.AlumniDto.Name;
                Alumni.Story = request.AlumniDto.Story;

                if (request.AlumniDto.File != null)
                {
                    var photoUploadResult = await _photoAccessor.UpdatePhoto(request.AlumniDto.File, Alumni.PhotoId);

                    if (photoUploadResult == null)
                        return Result<AlumniDto>.Failure("Creation Failed");




                    Alumni.Photo = new Photo
                    {
                        Url = photoUploadResult.Url,
                        Id = photoUploadResult.PublicId
                    };
                    Alumni.PhotoId = photoUploadResult.PublicId;
                }

                var sample = _mapper.Map<AlumniDto>(Alumni);

                _unitOfWork.AlumniRepository.Update(Alumni);

                if (await _unitOfWork.Save() > 0)
                {
                    return Result<AlumniDto>.Success(sample);
                }

                return Result<AlumniDto>.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result<AlumniDto>.Failure($"Update failed: {ex.Message}");
            }
        }
    }
}
