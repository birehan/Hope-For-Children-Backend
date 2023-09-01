using Domain.Common;

namespace Domain
{
    public class Category : BaseDomainEntity
    {
        public string Title { get; set; }
        public List<Photo> Photos { get; set; } = new List<Photo>();
        public string MainPhotoUrl { get; set; }

    }
}
