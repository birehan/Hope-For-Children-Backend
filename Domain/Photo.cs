namespace Domain
{
    public class Photo
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public Guid StaffId { get; set; }

        public Staff Staff { get; set; }


    }
}