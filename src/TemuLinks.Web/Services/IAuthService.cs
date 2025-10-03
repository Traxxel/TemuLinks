using TemuLinks.Web.Models;

namespace TemuLinks.Web.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginModel loginModel);
        Task LogoutAsync();
        bool IsAuthenticated { get; }
        string? Username { get; }
    }
}
