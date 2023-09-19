using Domain.Common;

namespace Domain
{
    public class Category : BaseDomainEntity
    {
        public string Title { get; set; }
        public List<GalleryPhoto> Photos { get; set; } = new List<GalleryPhoto>();
        public string MainPhotoUrl { get; set; }

    }
}
