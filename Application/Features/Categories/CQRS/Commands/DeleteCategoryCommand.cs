using Application.Contracts.Persistence;
using Application.Interfaces;
using Application.Responses;
using AutoMapper;
using MediatR;

namespace Application.Features.Categories.CQRS.Commands
{
    public class DeleteCategoryCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoAccessor _photoAccessor;

        public DeleteCategoryCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IPhotoAccessor photoAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _photoAccessor = photoAccessor;
        }

        public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var Category = await _unitOfWork.CategoryRepository.GetCategoryWithPhotos(request.Id);

            if (Category == null)
                return Result<Unit>.Failure("Object doesn't exist");

            foreach (var photo in Category.Photos)
            {
                if (await _photoAccessor.DeletePhoto(photo.Id) == null)
                    return Result<Unit>.Failure("Error while deleting Photos");
            }

            await _unitOfWork.CategoryRepository.Delete(Category);

            if (await _unitOfWork.Save() > 0)
                return Result<Unit>.Success(Unit.Value);

            return Result<Unit>.Failure("Error while deleting the object");
        }
    }
}
