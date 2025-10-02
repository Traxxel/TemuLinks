using System.Text;
using System.Text.Json;
using TemuLinks.Web.Models;

namespace TemuLinks.Web.Services;

public class TemuLinksApiService : ITemuLinksApiService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TemuLinksApiService> _logger;

    public TemuLinksApiService(HttpClient httpClient, ILogger<TemuLinksApiService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<TemuLinkDto>> GetMyLinksAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/links");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TemuLinkDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<TemuLinkDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen der Links");
            return new List<TemuLinkDto>();
        }
    }

    public async Task<TemuLinkDto?> GetLinkByIdAsync(int id)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/links/{id}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;
                
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TemuLinkDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen des Links {Id}", id);
            return null;
        }
    }

    public async Task<TemuLinkDto> CreateLinkAsync(CreateTemuLinkDto link)
    {
        try
        {
            var json = JsonSerializer.Serialize(link);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("/links", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TemuLinkDto>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Fehler beim Erstellen des Links");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Erstellen des Links");
            throw;
        }
    }

    public async Task<TemuLinkDto> UpdateLinkAsync(int id, UpdateTemuLinkDto link)
    {
        try
        {
            var json = JsonSerializer.Serialize(link);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"/links/{id}", content);
            response.EnsureSuccessStatusCode();
            
            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TemuLinkDto>(responseJson, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Fehler beim Aktualisieren des Links");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Aktualisieren des Links {Id}", id);
            throw;
        }
    }

    public async Task<bool> DeleteLinkAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/links/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Löschen des Links {Id}", id);
            return false;
        }
    }

    public async Task<List<TemuLinkDto>> GetPublicLinksAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/links/public");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TemuLinkDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<TemuLinkDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen der öffentlichen Links");
            return new List<TemuLinkDto>();
        }
    }

    public async Task<int> GetLinksCountAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/links/count");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<int>(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen der Link-Anzahl");
            return 0;
        }
    }

    public async Task<List<ApiKeyDto>> GetMyApiKeysAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/apikeys");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ApiKeyDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<ApiKeyDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Abrufen der API-Schlüssel");
            return new List<ApiKeyDto>();
        }
    }

    public async Task<ApiKeyDto> CreateApiKeyAsync()
    {
        try
        {
            var response = await _httpClient.PostAsync("/apikeys", null);
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ApiKeyDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? throw new InvalidOperationException("Fehler beim Erstellen des API-Schlüssels");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Erstellen des API-Schlüssels");
            throw;
        }
    }

    public async Task<bool> DeleteApiKeyAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"/apikeys/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fehler beim Löschen des API-Schlüssels {Id}", id);
            return false;
        }
    }
}
