using System.ComponentModel.DataAnnotations;

namespace TemuLinks.DAL.Entities
{
    public class ApiKey
    {
        public int Id { get; set; }
        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        [MaxLength(255)]
        public string Key { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public bool IsActive { get; set; } = true;
        
        // Navigation Properties
        public virtual User User { get; set; } = null!;
    }
}
