
using Application.Contracts.Persistence;

namespace Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private IStaffRepository _staffRepository;
        private IAlumniRepository _alumniRepository;
        private IProjectRepository _projectRepository;
        private ICategoryRepository _categoryRepository;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public IStaffRepository StaffRepository
        {
            get { return _staffRepository ??= new StaffRepository(_context); }
        }

        public IAlumniRepository AlumniRepository
        {
            get { return _alumniRepository ??= new AlumniRepository(_context); }
        }

        public IProjectRepository ProjectRepository
        {
            get { return _projectRepository ??= new ProjectRepository(_context); }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                return _categoryRepository ??= new CategoryRepository(_context);
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
