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

        public async Task<Category> GetByTitleAsync(string title)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(s => s.Title == title);
        }
    }
}
