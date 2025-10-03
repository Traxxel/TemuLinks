using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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

        public record ProfileDto(int Id, string Email, string? FirstName, string? LastName);
        public record UpdateProfileRequest(string? FirstName, string? LastName);

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.FindFirstValue(ClaimTypes.Name) ?? User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet("me")]
        public async Task<ActionResult<ProfileDto>> GetMe()
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();
            return new ProfileDto(user.Id, user.Email, user.FirstName, user.LastName);
        }

        [HttpPut("me")]
        public async Task<IActionResult> UpdateMe([FromBody] UpdateProfileRequest request)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return NotFound();
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}


