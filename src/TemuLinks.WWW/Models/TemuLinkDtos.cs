namespace TemuLinks.WWW.Models
{
    public class TemuLinkDto
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateTemuLinkDto
    {
        public string Url { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
    }

    public class TemuLinkCountDto
    {
        public int Count { get; set; }
    }
}


