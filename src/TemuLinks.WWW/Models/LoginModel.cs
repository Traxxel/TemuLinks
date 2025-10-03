using System.ComponentModel.DataAnnotations;

namespace TemuLinks.WWW.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Benutzername ist erforderlich")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
