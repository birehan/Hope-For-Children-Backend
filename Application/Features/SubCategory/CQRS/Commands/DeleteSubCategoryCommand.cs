using Application.Contracts.Persistence;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.SubCategories.CQRS.Commands
{
    public class DeleteSubCategoryCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteSubCategoryCommandHandler : IRequestHandler<DeleteSubCategoryCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoAccessor _photoAccessor;

        public DeleteSubCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Unit>> Handle(DeleteSubCategoryCommand request, CancellationToken cancellationToken)
        {
            var subCategory = await _unitOfWork.SubCategoryRepository.GetSubCategoryWithPhotos(request.Id);

            if (subCategory == null)
                return Result<Unit>.Failure("Object doesn't exist");

            if (subCategory.MainPhoto != null)
            {
                if (await _photoAccessor.DeletePhoto(subCategory.MainPhoto.Id) == null)
                    return Result<Unit>.Failure("Error while deleting main photo");
            }

            foreach(var photo in subCategory.Photos)
            {
                if (await _photoAccessor.DeletePhoto(photo.Id) == null)
                    return Result<Unit>.Failure("Error while deleting Photos");
            }

            await _unitOfWork.SubCategoryRepository.Delete(subCategory);

            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Error while deleting the object");
        }
    }
}
