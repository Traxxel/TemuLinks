using TemuLinks.WWW.Models;

namespace TemuLinks.WWW.Services
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        string? Username { get; }
        string? Email { get; }
        string? JwtToken { get; }
        Task<bool> LoginAsync(LoginModel loginModel);
        Task LogoutAsync();
    }
}
