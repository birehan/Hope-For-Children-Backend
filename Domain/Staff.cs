using Domain.Common;

namespace Domain
{
    public class Staff : BaseDomainEntity
    {
        public enum Sector
        {
            BoardMember,
            ManagementMember,
            StaffMember,
            FormerMember
        }



        public Sector UserSector { get; set; }

        public string Name { get; set; }

        public string Title { get; set; }


        public string About { get; set; }

        public string PhotoId { get; set; }

        public Photo Photo { get; set; }

    }
}