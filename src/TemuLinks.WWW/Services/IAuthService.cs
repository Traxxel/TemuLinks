using TemuLinks.WWW.Models;

namespace TemuLinks.WWW.Services
{
    public interface IAuthService
    {
        bool IsAuthenticated { get; }
        string? Username { get; }
        string? JwtToken { get; }
        string? Role { get; }
        bool IsActive { get; }
        Task<bool> LoginAsync(LoginModel loginModel);
        Task LogoutAsync();
    }
}
