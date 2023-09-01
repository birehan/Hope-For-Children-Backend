using Domain;

namespace Application.Contracts.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetByTitleAsync(string title);
        Task<Category> GetCategoryWithPhotos(Guid CategoryId);
        Task<List<Category>> GetAllCategoryWithPhotos();
    }
}
