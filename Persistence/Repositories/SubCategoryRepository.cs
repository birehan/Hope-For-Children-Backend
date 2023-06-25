using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class SubCategoryRepository : GenericRepository<SubCategory>, ISubCategoryRepository
    {
        private readonly DataContext _dbContext;
        public SubCategoryRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<SubCategory>> GetAllSubCategoryWithPhotos()
        {
           return await _dbContext.SubCategories
                    .Include(s => s.MainPhoto)
                    .Include(s => s.Photos)
                    .ToListAsync();
        }

        public async Task<SubCategory> GetByTitleAsync(string title)
        {
            return await _dbContext.SubCategories.FirstOrDefaultAsync(s => s.Title == title);
        }

        public async Task<SubCategory> GetSubCategoryWithPhotos(Guid SubCategoryId)
        {
            return await _dbContext.SubCategories
                .Include(s => s.Photos)
                .Include(s => s.MainPhoto)
                .FirstOrDefaultAsync(s => s.Id == SubCategoryId);
        }
    }
}
