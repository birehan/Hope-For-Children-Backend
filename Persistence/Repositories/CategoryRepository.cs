using Application.Contracts.Persistence;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly DataContext _dbContext;
        public CategoryRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Category>> GetAllCategoryWithPhotos()
        {
            return await _dbContext.Categories
                     .Include(s => s.Photos)
                     .ToListAsync();
        }

        public async Task<Category> GetByTitleAsync(string title)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(s => s.Title == title);
        }

        public async Task<Category> GetCategoryWithPhotos(Guid Id)
        {
            return await _dbContext.Categories.Include(s => s.Photos).FirstOrDefaultAsync(s => s.Id == Id);
        }
    }
}
