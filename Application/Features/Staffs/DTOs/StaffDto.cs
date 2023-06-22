using Application.Features.Common;
using static Domain.Staff;

namespace Application.Features.Staffs.DTOs
{
    public class StaffDto : BaseDto, IStaffDto
    {

        public string Name { get; set; }
        public string About { get; set; }

        public string PhotoUrl { get; set; }

        public string UserSector { get; set; }

        public string Title { get; set; }




    }
}