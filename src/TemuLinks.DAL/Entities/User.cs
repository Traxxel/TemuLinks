using System.ComponentModel.DataAnnotations;

namespace TemuLinks.DAL.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string? FirstName { get; set; }
        
        [MaxLength(100)]
        public string? LastName { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        [Required]
        [MaxLength(50)]
        public string Role { get; set; } = "User";
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public virtual ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();
        public virtual ICollection<TemuLink> TemuLinks { get; set; } = new List<TemuLink>();
    }
}
