using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;


namespace Persistence.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {

        private readonly DataContext _dbContext;

        public ProjectRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Project>> GetAll()
        {
            return await _dbContext.Set<Project>().Include(x => x.Photo).AsNoTracking().ToListAsync();
        }
        public async Task<Project> Get(Guid id)
        {
            return await _dbContext.Set<Project>().Include(x => x.Photo).FirstOrDefaultAsync(b => b.Id == id);
        }


    }
}