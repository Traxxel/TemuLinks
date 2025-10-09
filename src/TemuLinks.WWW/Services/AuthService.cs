using System.Net.Http.Json;
using TemuLinks.WWW.Models;

namespace TemuLinks.WWW.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private bool _isAuthenticated = false;
        private string? _username;
        private string? _jwt;
        private string? _role;
        private bool _isActive;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public bool IsAuthenticated => _isAuthenticated;
        public string? Username => _username;
        public string? JwtToken => _jwt;
        public string? Role => _role;
        public bool IsActive => _isActive;

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            _username = (loginModel.Username ?? string.Empty).Trim();
            var payload = new { Username = _username, Password = (loginModel.Password ?? string.Empty).Trim() };
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", payload);
            if (!response.IsSuccessStatusCode) return false;
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (string.IsNullOrWhiteSpace(result?.Token)) return false;
            _jwt = result!.Token;
            _isAuthenticated = true;
            _role = null;
            _isActive = false;

            // Set default Authorization header for all subsequent requests
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_jwt}");

            try
            {
                var profile = await _httpClient.GetFromJsonAsync<ProfileDto>("api/profile/me");
                if (profile != null)
                {
                    _role = profile.Role;
                    _isActive = profile.IsActive;
                }
            }
            catch
            {
                // ignore, profile optional
            }
            return true;
        }

        public Task LogoutAsync()
        {
            _isAuthenticated = false;
            _username = null;
            _jwt = null;
            _role = null;
            _isActive = false;
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            return Task.CompletedTask;
        }

        private class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
        }

        private class ProfileDto
        {
            public int Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string Role { get; set; } = string.Empty;
            public bool IsActive { get; set; }
        }
    }
}
