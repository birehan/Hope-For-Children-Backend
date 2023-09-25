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
    public class CreateAlumniCommand : IRequest<Result<AlumniDto>>
    {
        public CreateAlumniDto AlumniDto { get; set; }
    }

    public class CreateAlumniCommandHandler : IRequestHandler<CreateAlumniCommand, Result<AlumniDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IPhotoAccessor _photoAccessor;


        public CreateAlumniCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;

        }

        public async Task<Result<AlumniDto>> Handle(CreateAlumniCommand request, CancellationToken cancellationToken)
        {

            try
            {

             

                var validator = new CreateAlumniDtoValidator();
                var validationResult = await validator.ValidateAsync(request.AlumniDto);

                if (!validationResult.IsValid)
                    return Result<AlumniDto>.Failure(validationResult.Errors[0].ErrorMessage);


                var Alumni = _mapper.Map<Alumni>(request.AlumniDto);
                var photoUploadResult = await _photoAccessor.AddPhoto(request.AlumniDto.File);

                if (photoUploadResult == null)
                    return Result<AlumniDto>.Failure("Creation Failed due to photo upload error.");

                Alumni.Photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId,
                };
                Alumni.PhotoId = photoUploadResult.PublicId;

                await _unitOfWork.AlumniRepository.Add(Alumni);

                if (await _unitOfWork.Save() > 0)
                    return Result<AlumniDto>.Success(_mapper.Map<AlumniDto>(Alumni));

                return Result<AlumniDto>.Failure("Creation Failed");
            }
            catch (Exception ex)
            {
                return Result<AlumniDto>.Failure($"Creation Failed: {ex.Message}");

            }
        }
    }

}
