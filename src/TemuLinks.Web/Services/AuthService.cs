using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TemuLinks.Web.Models;

namespace TemuLinks.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private bool _isAuthenticated = false;
        private string? _username;

        public AuthService(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public bool IsAuthenticated => _isAuthenticated;
        public string? Username => _username;

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            // TODO: Hier wird später die echte Authentifizierung implementiert
            // Für jetzt: Einfache Demo-Authentifizierung (gehärtet gegen Leerzeichen & Case)
            var username = (loginModel.Username ?? string.Empty).Trim();
            var password = (loginModel.Password ?? string.Empty).Trim();

            // Debug-Ausgaben
            Console.WriteLine($"[AuthService] Raw Username: '{loginModel.Username}'");
            Console.WriteLine($"[AuthService] Raw Password: '{loginModel.Password}'");
            Console.WriteLine($"[AuthService] Trimmed Username: '{username}'");
            Console.WriteLine($"[AuthService] Trimmed Password: '{password}'");
            Console.WriteLine($"[AuthService] Username length: {username.Length}");
            Console.WriteLine($"[AuthService] Password length: {password.Length}");

            if (string.Equals(username, "admin", StringComparison.OrdinalIgnoreCase)
                && string.Equals(password, "admin", StringComparison.Ordinal))
            {
                Console.WriteLine("[AuthService] Login successful!");
                _isAuthenticated = true;
                _username = username;
                
                // Authentication State aktualisieren
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, "Admin")
                }, "custom");
                
                var user = new ClaimsPrincipal(identity);
                ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUser(user);
                
                return true;
            }
            
            Console.WriteLine("[AuthService] Login failed - credentials don't match");
            return false;
        }

        public async Task LogoutAsync()
        {
            _isAuthenticated = false;
            _username = null;
            ((CustomAuthenticationStateProvider)_authenticationStateProvider).SetUser(null);
        }
    }
}
