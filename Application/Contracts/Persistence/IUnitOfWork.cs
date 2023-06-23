

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {

        IStaffRepository StaffRepository { get; }

        IAlumniRepository AlumniRepository { get; }

        IProjectRepository ProjectRepository { get; }


        Task<int> Save();

    }
}