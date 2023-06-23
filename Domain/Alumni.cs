using Domain.Common;

namespace Domain
{
    public class Alumni : BaseDomainEntity
    {

        public string Name { get; set; }

        public string Story { get; set; }

        public string PhotoId { get; set; }

        public Photo Photo { get; set; }

    }
}