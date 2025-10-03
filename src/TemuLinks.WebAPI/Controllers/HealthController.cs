using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly TemuLinksDbContext _dbContext;

        public HealthController(TemuLinksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/health
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(object), 200)]
        public async Task<IActionResult> Get()
        {
            try
            {
                // Prüfe einfache DB-Verbindung per leichten Query
                var canConnect = await _dbContext.Database.CanConnectAsync();
                return Ok(new
                {
                    status = "ok",
                    database = canConnect ? "connected" : "disconnected",
                    timeUtc = DateTime.UtcNow
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = "error", message = ex.Message });
            }
        }
    }
}


