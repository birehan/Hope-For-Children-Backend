// using Application.Contracts.Persistence;
// using Application.Features.SubCategories.DTOs;
// using Application.Interfaces;
// using Application.Responses;
// using AutoMapper;
// using Domain;
// using MediatR;
// using Microsoft.AspNetCore.Http;

// namespace Application.Features.SubCategories.CQRS.Commands
// {
//     public class UpdateSubCategoryCommand : IRequest<Result<Guid>>
//     {
//         public UpdateSubCategoryDto SubCategoryDto { get; set; }
//     }

//     public class UpdateSubCategoryCommandHandler : IRequestHandler<UpdateSubCategoryCommand, Result<Guid>>
//     {
//         private readonly IMapper _mapper;
//         private readonly IUnitOfWork _unitOfWork;
//         private readonly IPhotoAccessor _photoAccessor;

//         public UpdateSubCategoryCommandHandler(IMapper mapper, IUnitOfWork unitOfWork, IPhotoAccessor photoAccessor)
//         {
//             _mapper = mapper;
//             _unitOfWork = unitOfWork;
//             _photoAccessor = photoAccessor;
//         }

//         public async Task<Result<Guid>> Handle(UpdateSubCategoryCommand request, CancellationToken cancellationToken)
//         {
//             var existingSubCategory =  await _unitOfWork.SubCategoryRepository.Get(request.SubCategoryDto.Id);

//             if (existingSubCategory == null)
//                 return Result<Guid>.Failure("Object not found");

//             if (request.SubCategoryDto.Title != null)
//                 _mapper.Map(request.SubCategoryDto, existingSubCategory);

//             if (request.SubCategoryDto.MainPhoto != null)
//             {
//                 var mainPhotoResult = await _photoAccessor.AddPhoto(request.SubCategoryDto.MainPhoto);
//                 if (mainPhotoResult == null)
//                 {
//                     return Result<Guid>.Failure("Error uploading main photo");
//                 }

//                 existingSubCategory.MainPhoto = new Photo
//                 {
//                     Url = mainPhotoResult.Url,
//                     Id = mainPhotoResult.PublicId,
//                 };
//             }

//             if (request.SubCategoryDto.Photos != null)
//             {
//                 var photoUploadTasks = request.SubCategoryDto.Photos.Select(p => _photoAccessor.AddPhoto(p));
//                 var photoUploadResults = await Task.WhenAll(photoUploadTasks);

//                 if (photoUploadResults.Any(r => r == null))
//                 {
//                     return Result<Guid>.Failure("Error uploading one or more photos");
//                 }

//                 existingSubCategory.Photos.AddRange(photoUploadResults.Select(r => new Photo { Url = r.Url, Id = r.PublicId, }));
//             }


//             await _unitOfWork.SubCategoryRepository.Update(existingSubCategory);

//             if (await _unitOfWork.Save() > 0)
//             {
//                 return Result<Guid>.Success(existingSubCategory.Id);
//             }

//             return Result<Guid>.Failure("Error while saving changes");
//         }
//     }
// }
