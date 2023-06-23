namespace Domain
{
    public class ProjectFile
    {
        public string Id { get; set; }

        public string Url { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}