using TemuLinks.WWW.Models;

namespace TemuLinks.WWW.Services
{
    public class AuthService : IAuthService
    {
        private bool _isAuthenticated = false;
        private string? _username;

        public bool IsAuthenticated => _isAuthenticated;
        public string? Username => _username;

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            // TODO: Hier wird später die echte Authentifizierung implementiert
            // Für jetzt: Einfache Demo-Authentifizierung
            var username = (loginModel.Username ?? string.Empty).Trim();
            var password = (loginModel.Password ?? string.Empty).Trim();

            // Debug-Ausgaben
            Console.WriteLine($"[WWW AuthService] Raw Username: '{loginModel.Username}'");
            Console.WriteLine($"[WWW AuthService] Raw Password: '{loginModel.Password}'");
            Console.WriteLine($"[WWW AuthService] Trimmed Username: '{username}'");
            Console.WriteLine($"[WWW AuthService] Trimmed Password: '{password}'");
            Console.WriteLine($"[WWW AuthService] Username length: {username.Length}");
            Console.WriteLine($"[WWW AuthService] Password length: {password.Length}");

            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase)
                && string.Equals(password, "admin", StringComparison.Ordinal))
            {
                Console.WriteLine("[WWW AuthService] Login successful!");
                _isAuthenticated = true;
                _username = username;
                return true;
            }

            Console.WriteLine("[WWW AuthService] Login failed - credentials don't match");
            return false;
        }

        public async Task LogoutAsync()
        {
            _isAuthenticated = false;
            _username = null;
        }
    }
}
