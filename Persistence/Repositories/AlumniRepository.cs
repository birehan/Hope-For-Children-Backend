using Application.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;
using Domain;


namespace Persistence.Repositories
{
    public class AlumniRepository : GenericRepository<Alumni>, IAlumniRepository
    {

        private readonly DataContext _dbContext;

        public AlumniRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Alumni>> GetAll()
        {
            return await _dbContext.Set<Alumni>().Include(x => x.Photo).AsNoTracking().ToListAsync();
        }
        public async Task<Alumni> Get(Guid id)
        {
            return await _dbContext.Set<Alumni>().Include(x => x.Photo).FirstOrDefaultAsync(b => b.Id == id);
        }

      

    }
}