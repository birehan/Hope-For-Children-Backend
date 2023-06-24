namespace Domain
{
    public class Photo
    {
        public string Id { get; set; }
        public string Url { get; set; }

        public Guid? StaffId { get; set; }

        public Staff? Staff { get; set; }

        public Guid? AlumniId { get; set; }

        public Alumni? Alumni { get; set; }

        public Guid? ProjectId { get; set; }

        public Project? Project { get; set; }

    }
}
