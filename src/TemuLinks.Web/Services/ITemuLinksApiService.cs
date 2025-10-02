using TemuLinks.Web.Models;

namespace TemuLinks.Web.Services;

public interface ITemuLinksApiService
{
    Task<List<TemuLinkDto>> GetMyLinksAsync();
    Task<TemuLinkDto?> GetLinkByIdAsync(int id);
    Task<TemuLinkDto> CreateLinkAsync(CreateTemuLinkDto link);
    Task<TemuLinkDto> UpdateLinkAsync(int id, UpdateTemuLinkDto link);
    Task<bool> DeleteLinkAsync(int id);
    Task<List<TemuLinkDto>> GetPublicLinksAsync();
    Task<int> GetLinksCountAsync();
    Task<List<ApiKeyDto>> GetMyApiKeysAsync();
    Task<ApiKeyDto> CreateApiKeyAsync();
    Task<bool> DeleteApiKeyAsync(int id);
}
