

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {

        IStaffRepository StaffRepository{get;} 

        IAlumniRepository AlumniRepository{get;} 

        
        Task<int> Save();

    }
}