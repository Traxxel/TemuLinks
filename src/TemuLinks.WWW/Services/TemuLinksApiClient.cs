using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using TemuLinks.WWW.Models;

namespace TemuLinks.WWW.Services
{
    public interface ITemuLinksApiClient
    {
        Task<int?> GetLinkCountAsync(CancellationToken cancellationToken = default);
        Task<List<TemuLinkDto>?> GetLinksAsync(CancellationToken cancellationToken = default);
        Task<TemuLinkDto?> CreateLinkAsync(CreateTemuLinkDto dto, CancellationToken cancellationToken = default);
    }

    public class TemuLinksApiClient : ITemuLinksApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public TemuLinksApiClient(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        private async Task<bool> EnsureApiKeyAsync()
        {
            var apiKey = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "temulinks_api_key");
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return false;
            }

            if (_httpClient.DefaultRequestHeaders.Contains("X-API-Key"))
            {
                _httpClient.DefaultRequestHeaders.Remove("X-API-Key");
            }
            _httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
            return true;
        }

        public async Task<int?> GetLinkCountAsync(CancellationToken cancellationToken = default)
        {
            if (!await EnsureApiKeyAsync()) return null;
            var response = await _httpClient.GetAsync("api/temulinks/count", cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            var dto = await response.Content.ReadFromJsonAsync<TemuLinkCountDto>(cancellationToken: cancellationToken);
            return dto?.Count;
        }

        public async Task<List<TemuLinkDto>?> GetLinksAsync(CancellationToken cancellationToken = default)
        {
            if (!await EnsureApiKeyAsync()) return null;
            return await _httpClient.GetFromJsonAsync<List<TemuLinkDto>>("api/temulinks", cancellationToken);
        }

        public async Task<TemuLinkDto?> CreateLinkAsync(CreateTemuLinkDto dto, CancellationToken cancellationToken = default)
        {
            if (!await EnsureApiKeyAsync()) return null;
            var response = await _httpClient.PostAsJsonAsync("api/temulinks", dto, cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<TemuLinkDto>(cancellationToken: cancellationToken);
        }
    }
}


