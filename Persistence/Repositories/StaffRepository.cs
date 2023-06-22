using Microsoft.EntityFrameworkCore;
using Application.Contracts.Persistence;
using Domain;
using static Domain.Staff;

namespace Persistence.Repositories
{
    public class StaffRepository : GenericRepository<Domain.Staff>, IStaffRepository
    {

        private readonly DataContext _dbContext;

        public StaffRepository(DataContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Staff>> GetAll()
        {
            return await _dbContext.Set<Staff>().Include(x => x.Photo).AsNoTracking().ToListAsync();
        }
        public async Task<Staff> Get(Guid id)
        {
            return await _dbContext.Set<Staff>().Include(x => x.Photo).FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IReadOnlyList<Staff>> GetStaffsBySector(Sector sector)
        {
            return await _dbContext.Set<Staff>()
                .Include(x => x.Photo)
                .Where(s => s.UserSector == sector)
                .AsNoTracking()
                .ToListAsync();
        }

    }
}