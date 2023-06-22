
using Application.Contracts.Persistence;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;


        private IStaffRepository _staffRepository;



        public UnitOfWork(DataContext context)
        {
            _context = context;
        }


   

        public IStaffRepository StaffRepository
        {
            get
            {
                return _staffRepository ??= new StaffRepository(_context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}