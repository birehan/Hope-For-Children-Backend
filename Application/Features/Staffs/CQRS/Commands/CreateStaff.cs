using MediatR;
using Application.Features.Staffs.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;
using Application.Interfaces;
using HFC.Application.Features.Staffs.DTOs.Validators;
using Domain;

namespace Application.Features.Staffs.CQRS.Commands
{
    public class CreateStaffCommand : IRequest<Result<StaffDto>>
    {
        public CreateStaffDto StaffDto { get; set; }
    }

    public class CreateStaffCommandHandler : IRequestHandler<CreateStaffCommand, Result<StaffDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IPhotoAccessor _photoAccessor;


        public CreateStaffCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;

        }

        public async Task<Result<StaffDto>> Handle(CreateStaffCommand request, CancellationToken cancellationToken)
        {

            try
            {

                var validator = new CreateStaffDtoValidator();
                var validationResult = await validator.ValidateAsync(request.StaffDto);

                if (!validationResult.IsValid)
                    return Result<StaffDto>.Failure(validationResult.Errors[0].ErrorMessage);


                var Staff = _mapper.Map<Staff>(request.StaffDto);
                var photoUploadResult = await _photoAccessor.AddPhoto(request.StaffDto.File);

                if (photoUploadResult == null)
                    return Result<StaffDto>.Failure("Creation Failed due to photo upload error.");

                Staff.Photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId,
                };
                Staff.PhotoId = photoUploadResult.PublicId;

                await _unitOfWork.StaffRepository.Add(Staff);

                if (await _unitOfWork.Save() > 0)
                    return Result<StaffDto>.Success(_mapper.Map<StaffDto>(Staff));

                return Result<StaffDto>.Failure("Creation Failed");
            }
            catch (Exception ex)
            {
                return Result<StaffDto>.Failure($"Creation Failed: {ex.Message}");

            }
        }
    }

}
