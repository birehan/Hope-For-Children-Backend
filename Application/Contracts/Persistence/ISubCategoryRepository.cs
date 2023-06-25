using Domain;

namespace Application.Contracts.Persistence
{
    public interface ISubCategoryRepository : IGenericRepository<SubCategory>
    {
        Task<SubCategory> GetByTitleAsync(string title);
        Task<SubCategory> GetSubCategoryWithPhotos(Guid SubCategoryId);
        Task<List<SubCategory>> GetAllSubCategoryWithPhotos();
    }
}
