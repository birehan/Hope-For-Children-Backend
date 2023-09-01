

namespace Application.Contracts.Persistence
{
    public interface IUnitOfWork : IDisposable
    {

        IStaffRepository StaffRepository { get; }

        IAlumniRepository AlumniRepository { get; }

        IProjectRepository ProjectRepository { get; }


        ICategoryRepository CategoryRepository { get; }

        Task<int> Save();

    }
}
