using Domain.Common;

namespace Domain
{
    public class Category : BaseDomainEntity
    {
        public string Title { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public Category()
        {
            SubCategories = new List<SubCategory>();
        }
    }
}
