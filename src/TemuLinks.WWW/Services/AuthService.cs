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

            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase)
                && string.Equals(password, "admin", StringComparison.Ordinal))
            {
                _isAuthenticated = true;
                _username = username;
                return true;
            }
            return false;
        }

        public async Task LogoutAsync()
        {
            _isAuthenticated = false;
            _username = null;
        }
    }
}
