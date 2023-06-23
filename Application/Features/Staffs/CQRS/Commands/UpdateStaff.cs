using MediatR;
using Application.Features.Staffs.DTOs;
using Application.Responses;
using Application.Contracts.Persistence;
using AutoMapper;
using Application.Interfaces;
using HFC.Application.Features.Staffs.DTOs.Validators;
using Domain;
using static Domain.Staff;

namespace Application.Features.Staffs.CQRS.Commands
{
    public class UpdateStaffCommand : IRequest<Result<StaffDto>>
    {
        public UpdateStaffDto StaffDto { get; set; }
    }

    public class UpdateStaffCommandHandler : IRequestHandler<UpdateStaffCommand, Result<StaffDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPhotoAccessor _photoAccessor;

        public UpdateStaffCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<StaffDto>> Handle(UpdateStaffCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateStaffDtoValidator();
                var validationResult = await validator.ValidateAsync(request.StaffDto);

                if (!validationResult.IsValid)
                    return Result<StaffDto>.Failure(validationResult.Errors[0].ErrorMessage);

                var staff = await _unitOfWork.StaffRepository.Get(request.StaffDto.Id);

                if (staff == null)
                    return Result<StaffDto>.Failure("Staff not found");

                staff.Name = request.StaffDto.Name;
                staff.About = request.StaffDto.About;
                staff.Title = request.StaffDto.Title;
                staff.UserSector = Enum.Parse<Sector>(request.StaffDto.UserSector);

                if (request.StaffDto.File != null)
                {
                    var photoUploadResult = await _photoAccessor.UpdatePhoto(request.StaffDto.File, staff.PhotoId);

                    if (photoUploadResult == null)
                        return Result<StaffDto>.Failure("Creation Failed");

                    staff.Photo = new Photo
                    {
                        Url = photoUploadResult.Url,
                        Id = photoUploadResult.PublicId
                    };
                    staff.PhotoId = photoUploadResult.PublicId;
                }

                _unitOfWork.StaffRepository.Update(staff);

                if (await _unitOfWork.Save() > 0)
                    return Result<StaffDto>.Success(_mapper.Map<StaffDto>(staff));

                return Result<StaffDto>.Failure("Update failed");
            }
            catch (Exception ex)
            {
                return Result<StaffDto>.Failure($"Update failed: {ex.Message}");
            }
        }
    }
}
