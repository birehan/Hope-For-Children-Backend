using Domain;

namespace Application.Contracts.Persistence
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<Category> GetByTitleAsync(string title);
    }
}
