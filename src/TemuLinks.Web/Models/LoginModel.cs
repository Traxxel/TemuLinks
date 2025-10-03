using System.ComponentModel.DataAnnotations;

namespace TemuLinks.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Benutzername ist erforderlich")]
        [Display(Name = "Benutzername")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Passwort ist erforderlich")]
        [DataType(DataType.Password)]
        [Display(Name = "Passwort")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Angemeldet bleiben")]
        public bool RememberMe { get; set; }
    }
}
