using Domain.Common;

namespace Domain
{
    public class SubCategory : BaseDomainEntity
    {
        public string Title { get; set; }
        public List<Photo> Photos { get; set; }
        public Photo MainPhoto { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        public SubCategory()
        {
            Photos =  new List<Photo>();
        }
    }
}
