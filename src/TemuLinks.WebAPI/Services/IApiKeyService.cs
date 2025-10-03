using TemuLinks.DAL.Entities;
using TemuLinks.WebAPI.DTOs;

namespace TemuLinks.WebAPI.Services
{
    public interface IApiKeyService
    {
        Task<User?> GetUserByApiKeyAsync(string apiKey);
        Task<GenerateApiKeyResponse> GenerateApiKeyAsync(string userEmail);
        Task<IEnumerable<ApiKeyDto>> GetUserApiKeysAsync(string userEmail);
        Task<bool> DeleteApiKeyAsync(int apiKeyId, string userEmail);
        Task<IEnumerable<ApiKeyDto>> GetUserApiKeysByUserIdAsync(int userId);
        Task<GenerateApiKeyResponse> GenerateApiKeyForUserIdAsync(int userId);
    }
}
