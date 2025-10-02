using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;
using TemuLinks.DAL.Entities;
using System.Security.Cryptography;
using System.Text;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiKeysController : ControllerBase
    {
        private readonly TemuLinksDbContext _context;

        public ApiKeysController(TemuLinksDbContext context)
        {
            _context = context;
        }

        // POST: api/apikeys/generate
        [HttpPost("generate")]
        public async Task<ActionResult<object>> GenerateApiKey([FromBody] GenerateApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.UserEmail))
            {
                return BadRequest("User email is required");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.UserEmail && u.IsActive);

            if (user == null)
            {
                return NotFound("User not found or inactive");
            }

            // Generate a new API key
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

            return Ok(new { apiKey, userId = user.Id, createdAt = newApiKey.CreatedAt });
        }

        // GET: api/apikeys/user/{email}
        [HttpGet("user/{email}")]
        public async Task<ActionResult<IEnumerable<object>>> GetUserApiKeys(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            if (user == null)
            {
                return NotFound("User not found or inactive");
            }

            var apiKeys = await _context.ApiKeys
                .Where(k => k.UserId == user.Id)
                .OrderByDescending(k => k.CreatedAt)
                .Select(k => new
                {
                    k.Id,
                    k.Key,
                    k.CreatedAt,
                    k.IsActive
                })
                .ToListAsync();

            return Ok(apiKeys);
        }

        // DELETE: api/apikeys/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApiKey(int id, [FromBody] DeleteApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.UserEmail))
            {
                return BadRequest("User email is required");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.UserEmail && u.IsActive);

            if (user == null)
            {
                return NotFound("User not found or inactive");
            }

            var apiKey = await _context.ApiKeys
                .FirstOrDefaultAsync(k => k.Id == id && k.UserId == user.Id);

            if (apiKey == null)
            {
                return NotFound("API Key not found");
            }

            _context.ApiKeys.Remove(apiKey);
            await _context.SaveChangesAsync();

            return NoContent();
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

    public class GenerateApiKeyRequest
    {
        public string UserEmail { get; set; } = string.Empty;
    }

    public class DeleteApiKeyRequest
    {
        public string UserEmail { get; set; } = string.Empty;
    }
}
