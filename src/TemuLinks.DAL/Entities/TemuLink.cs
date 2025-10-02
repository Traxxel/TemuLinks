using System.ComponentModel.DataAnnotations;

namespace TemuLinks.DAL.Entities
{
    public class TemuLink
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(2000)]
        public string Url { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        public bool IsPublic { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
    }
}
