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
        private readonly IAuthService _authService;

        public TemuLinksApiClient(HttpClient httpClient, IJSRuntime jsRuntime, IAuthService authService)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
            _authService = authService;
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

        private void EnsureBearer()
        {
            if (!string.IsNullOrWhiteSpace(_authService.JwtToken))
            {
                _httpClient.DefaultRequestHeaders.Remove("Authorization");
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_authService.JwtToken}");
            }
        }

        public async Task<int?> GetLinkCountAsync(CancellationToken cancellationToken = default)
        {
            EnsureBearer();
            if (!await EnsureApiKeyAsync()) return null;
            var response = await _httpClient.GetAsync("api/temulinks/count", cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            var dto = await response.Content.ReadFromJsonAsync<TemuLinkCountDto>(cancellationToken: cancellationToken);
            return dto?.Count;
        }

        public async Task<List<TemuLinkDto>?> GetLinksAsync(CancellationToken cancellationToken = default)
        {
            EnsureBearer();
            if (!await EnsureApiKeyAsync()) return null;
            return await _httpClient.GetFromJsonAsync<List<TemuLinkDto>>("api/temulinks", cancellationToken);
        }

        public async Task<TemuLinkDto?> CreateLinkAsync(CreateTemuLinkDto dto, CancellationToken cancellationToken = default)
        {
            EnsureBearer();
            if (!await EnsureApiKeyAsync()) return null;
            var response = await _httpClient.PostAsJsonAsync("api/temulinks", dto, cancellationToken);
            if (!response.IsSuccessStatusCode) return null;
            return await response.Content.ReadFromJsonAsync<TemuLinkDto>(cancellationToken: cancellationToken);
        }
    }
}


