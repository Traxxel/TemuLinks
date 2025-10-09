using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TemuLinks.DAL;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly TemuLinksDbContext _db;

        public UsersController(TemuLinksDbContext db)
        {
            _db = db;
        }

        public record UserListItemDto(int Id, string Username, string Email, string? FirstName, string? LastName, bool IsActive, string Role);
        public record UpdateUserRequest([MaxLength(100)] string? FirstName, [MaxLength(100)] string? LastName);

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserListItemDto>>> GetUsers()
        {
            var users = await _db.Users
                .OrderBy(u => u.Username)
                .Select(u => new UserListItemDto(u.Id, u.Username, u.Email, u.FirstName, u.LastName, u.IsActive, u.Role))
                .ToListAsync();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/activate")]
        public async Task<IActionResult> ActivateUser(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            user.IsActive = true;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("{id}/deactivate")]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            user.IsActive = false;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}


