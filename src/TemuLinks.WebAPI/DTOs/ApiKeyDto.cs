namespace TemuLinks.WebAPI.DTOs
{
    public class ApiKeyDto
    {
        public int Id { get; set; }
        public string Key { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class GenerateApiKeyRequest
    {
        public string UserEmail { get; set; } = string.Empty;
    }

    public class DeleteApiKeyRequest
    {
        public string UserEmail { get; set; } = string.Empty;
    }

    public class GenerateApiKeyResponse
    {
        public string ApiKey { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
