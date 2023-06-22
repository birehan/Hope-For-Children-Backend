

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {

        IStaffRepository StaffRepository{get;} 


        
        Task<int> Save();

    }
}