using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;
using TemuLinks.DAL.Entities;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemuLinksController : ControllerBase
    {
        private readonly TemuLinksDbContext _context;

        public TemuLinksController(TemuLinksDbContext context)
        {
            _context = context;
        }

        // GET: api/temulinks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemuLink>>> GetTemuLinks()
        {
            var apiKey = Request.Headers["X-API-Key"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized("API Key is required");
            }

            var user = await _context.Users
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.ApiKeys.Any(k => k.Key == apiKey && k.IsActive));

            if (user == null)
            {
                return Unauthorized("Invalid API Key");
            }

            var links = await _context.TemuLinks
                .Where(l => l.UserId == user.Id)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();

            return Ok(links);
        }

        // GET: api/temulinks/public
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<object>>> GetPublicTemuLinks()
        {
            var publicLinks = await _context.TemuLinks
                .Include(l => l.User)
                .Where(l => l.IsPublic)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new
                {
                    l.Id,
                    l.Url,
                    l.Description,
                    l.CreatedAt,
                    UserName = l.User.FirstName + " " + l.User.LastName
                })
                .ToListAsync();

            return Ok(publicLinks);
        }

        // GET: api/temulinks/count
        [HttpGet("count")]
        public async Task<ActionResult<object>> GetTemuLinksCount()
        {
            var apiKey = Request.Headers["X-API-Key"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized("API Key is required");
            }

            var user = await _context.Users
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.ApiKeys.Any(k => k.Key == apiKey && k.IsActive));

            if (user == null)
            {
                return Unauthorized("Invalid API Key");
            }

            var count = await _context.TemuLinks
                .CountAsync(l => l.UserId == user.Id);

            return Ok(new { count });
        }

        // POST: api/temulinks
        [HttpPost]
        public async Task<ActionResult<TemuLink>> PostTemuLink(TemuLink temuLink)
        {
            var apiKey = Request.Headers["X-API-Key"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized("API Key is required");
            }

            var user = await _context.Users
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.ApiKeys.Any(k => k.Key == apiKey && k.IsActive));

            if (user == null)
            {
                return Unauthorized("Invalid API Key");
            }

            temuLink.UserId = user.Id;
            temuLink.CreatedAt = DateTime.UtcNow;

            _context.TemuLinks.Add(temuLink);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTemuLink", new { id = temuLink.Id }, temuLink);
        }

        // GET: api/temulinks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TemuLink>> GetTemuLink(int id)
        {
            var apiKey = Request.Headers["X-API-Key"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized("API Key is required");
            }

            var user = await _context.Users
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.ApiKeys.Any(k => k.Key == apiKey && k.IsActive));

            if (user == null)
            {
                return Unauthorized("Invalid API Key");
            }

            var temuLink = await _context.TemuLinks
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == user.Id);

            if (temuLink == null)
            {
                return NotFound();
            }

            return temuLink;
        }

        // PUT: api/temulinks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTemuLink(int id, TemuLink temuLink)
        {
            var apiKey = Request.Headers["X-API-Key"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized("API Key is required");
            }

            var user = await _context.Users
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.ApiKeys.Any(k => k.Key == apiKey && k.IsActive));

            if (user == null)
            {
                return Unauthorized("Invalid API Key");
            }

            if (id != temuLink.Id)
            {
                return BadRequest();
            }

            var existingLink = await _context.TemuLinks
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == user.Id);

            if (existingLink == null)
            {
                return NotFound();
            }

            existingLink.Url = temuLink.Url;
            existingLink.Description = temuLink.Description;
            existingLink.IsPublic = temuLink.IsPublic;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TemuLinkExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/temulinks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemuLink(int id)
        {
            var apiKey = Request.Headers["X-API-Key"].FirstOrDefault();
            if (string.IsNullOrEmpty(apiKey))
            {
                return Unauthorized("API Key is required");
            }

            var user = await _context.Users
                .Include(u => u.ApiKeys)
                .FirstOrDefaultAsync(u => u.ApiKeys.Any(k => k.Key == apiKey && k.IsActive));

            if (user == null)
            {
                return Unauthorized("Invalid API Key");
            }

            var temuLink = await _context.TemuLinks
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == user.Id);

            if (temuLink == null)
            {
                return NotFound();
            }

            _context.TemuLinks.Remove(temuLink);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TemuLinkExists(int id)
        {
            return _context.TemuLinks.Any(e => e.Id == id);
        }
    }
}
