using Domain.Common;
namespace Domain
{
    public class Project : BaseDomainEntity
    {
        public string Title { get; set; }


        public string Description { get; set; }

        public string PhotoId { get; set; }

        public Photo Photo { get; set; }


        public string Content { get; set; }
    }
}