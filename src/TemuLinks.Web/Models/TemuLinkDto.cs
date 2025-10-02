namespace TemuLinks.Web.Models;

public class TemuLinkDto
{
    public int Id { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UserEmail { get; set; } = string.Empty;
}

public class CreateTemuLinkDto
{
    public string Url { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}

public class UpdateTemuLinkDto
{
    public string Description { get; set; } = string.Empty;
    public bool IsPublic { get; set; }
}

public class ApiKeyDto
{
    public int Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsActive { get; set; }
}
