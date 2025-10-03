using Microsoft.AspNetCore.Mvc;
using TemuLinks.WebAPI.DTOs;
using TemuLinks.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiKeysController : ControllerBase
    {
        private readonly IApiKeyService _apiKeyService;
        private readonly IConfiguration _config;

        public ApiKeysController(IApiKeyService apiKeyService, IConfiguration config)
        {
            _apiKeyService = apiKeyService;
            _config = config;
        }

        // POST: api/apikeys/generate
        [HttpPost("generate")]
        public async Task<ActionResult<GenerateApiKeyResponse>> GenerateApiKey([FromBody] GenerateApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.UserEmail))
            {
                return BadRequest("User email is required");
            }

            try
            {
                var response = await _apiKeyService.GenerateApiKeyAsync(request.UserEmail);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // GET: api/apikeys/user/{email}
        [HttpGet("user/{email}")]
        public async Task<ActionResult<IEnumerable<ApiKeyDto>>> GetUserApiKeys(string email)
        {
            try
            {
                var apiKeys = await _apiKeyService.GetUserApiKeysAsync(email);
                return Ok(apiKeys);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE: api/apikeys/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApiKey(int id, [FromBody] DeleteApiKeyRequest request)
        {
            if (string.IsNullOrEmpty(request.UserEmail))
            {
                return BadRequest("User email is required");
            }

            try
            {
                var deleted = await _apiKeyService.DeleteApiKeyAsync(id, request.UserEmail);
                if (!deleted)
                {
                    return NotFound("API Key not found");
                }
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // Authenticated user endpoints (JWT)
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<IEnumerable<ApiKeyDto>>> GetMyApiKeys()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            try
            {
                var apiKeys = await _apiKeyService.GetUserApiKeysAsync(email);
                return Ok(apiKeys);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize]
        [HttpPost("me")]
        public async Task<ActionResult<GenerateApiKeyResponse>> GenerateMyApiKey()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            try
            {
                var response = await _apiKeyService.GenerateApiKeyAsync(email);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
