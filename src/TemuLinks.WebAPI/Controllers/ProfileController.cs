using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using TemuLinks.DAL;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProfileController : ControllerBase
    {
        private readonly TemuLinksDbContext _db;

        public ProfileController(TemuLinksDbContext db)
        {
            _db = db;
        }

        public record ProfileDto(int Id, string? FirstName, string? LastName, string Role, bool IsActive);
        public record UpdateProfileRequest(string? FirstName, string? LastName);

        private int GetUserId()
        {
            // Try common claim types to be robust against inbound claim mapping
            var idCandidates = new[]
            {
                User.FindFirstValue(JwtRegisteredClaimNames.Sub),
                User.FindFirstValue(ClaimTypes.NameIdentifier),
                User.FindFirstValue(ClaimTypes.Name)
            };

            foreach (var candidate in idCandidates)
            {
                if (!string.IsNullOrWhiteSpace(candidate) && int.TryParse(candidate, out var parsed))
                {
                    return parsed;
                }
            }

            return 0;
        }

        [HttpGet("me")]
        public async Task<ActionResult<ProfileDto>> GetMe()
        {
            var userId = GetUserId();
            var user = userId > 0
                ? await _db.Users.FirstOrDefaultAsync(u => u.Id == userId)
                : null;

            // Keine Fallback-Suche per Email mehr

            if (user == null) return NotFound();
            return new ProfileDto(user.Id, user.FirstName, user.LastName, user.Role, user.IsActive);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequest request)
        {
            var userId = GetUserId();
            var user = userId > 0
                ? await _db.Users.FirstOrDefaultAsync(u => u.Id == userId)
                : null;

            // Keine Fallback-Suche per Email mehr

            if (user == null) return NotFound();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}


