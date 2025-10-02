using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;
using TemuLinks.DAL.Entities;
using TemuLinks.WebAPI.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace TemuLinks.WebAPI.Services
{
    public class ApiKeyService : IApiKeyService
    {
        private readonly TemuLinksDbContext _context;

        public ApiKeyService(TemuLinksDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByApiKeyAsync(string apiKey)
        {
            return await _context.Users
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.ApiKeys.Any(k => k.Key == apiKey && k.IsActive));
        }

        public async Task<GenerateApiKeyResponse> GenerateApiKeyAsync(string userEmail)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail && u.IsActive);

            if (user == null)
                throw new ArgumentException("User not found or inactive");

            var apiKey = GenerateSecureApiKey();

            var newApiKey = new ApiKey
            {
                UserId = user.Id,
                Key = apiKey,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.ApiKeys.Add(newApiKey);
            await _context.SaveChangesAsync();

            return new GenerateApiKeyResponse
            {
                ApiKey = apiKey,
                UserId = user.Id,
                CreatedAt = newApiKey.CreatedAt
            };
        }

        public async Task<IEnumerable<ApiKeyDto>> GetUserApiKeysAsync(string userEmail)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail && u.IsActive);

            if (user == null)
                throw new ArgumentException("User not found or inactive");

            var apiKeys = await _context.ApiKeys
                .Where(k => k.UserId == user.Id)
                .OrderByDescending(k => k.CreatedAt)
                .Select(k => new ApiKeyDto
                {
                    Id = k.Id,
                    Key = k.Key,
                    CreatedAt = k.CreatedAt,
                    IsActive = k.IsActive
                })
                .ToListAsync();

            return apiKeys;
        }

        public async Task<bool> DeleteApiKeyAsync(int apiKeyId, string userEmail)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == userEmail && u.IsActive);

            if (user == null)
                return false;

            var apiKey = await _context.ApiKeys
                .FirstOrDefaultAsync(k => k.Id == apiKeyId && k.UserId == user.Id);

            if (apiKey == null)
                return false;

            _context.ApiKeys.Remove(apiKey);
            await _context.SaveChangesAsync();

            return true;
        }

        private string GenerateSecureApiKey()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            var result = new StringBuilder();
            
            for (int i = 0; i < 32; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }
            
            return result.ToString();
        }
    }
}
