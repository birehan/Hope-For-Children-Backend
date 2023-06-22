using Domain;
using static Domain.Staff;

namespace Application.Contracts.Persistence
{
    public interface IStaffRepository : IGenericRepository<Staff>
    {
        Task<IReadOnlyList<Staff>> GetStaffsBySector(Sector sector);


    }
}