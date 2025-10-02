using Microsoft.AspNetCore.Mvc;
using TemuLinks.DAL.Entities;
using TemuLinks.WebAPI.DTOs;
using TemuLinks.WebAPI.Services;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TemuLinksController : ControllerBase
    {
        private readonly ITemuLinkService _temuLinkService;

        public TemuLinksController(ITemuLinkService temuLinkService)
        {
            _temuLinkService = temuLinkService;
        }

        // GET: api/temulinks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TemuLinkDto>>> GetTemuLinks()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var links = await _temuLinkService.GetUserLinksAsync(userId);
            return Ok(links);
        }

        // GET: api/temulinks/public
        [HttpGet("public")]
        public async Task<ActionResult<IEnumerable<TemuLinkDto>>> GetPublicTemuLinks()
        {
            var publicLinks = await _temuLinkService.GetPublicLinksAsync();
            return Ok(publicLinks);
        }

        // GET: api/temulinks/count
        [HttpGet("count")]
        public async Task<ActionResult<TemuLinkCountDto>> GetTemuLinksCount()
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var count = await _temuLinkService.GetUserLinkCountAsync(userId);
            return Ok(new TemuLinkCountDto { Count = count });
        }

        // POST: api/temulinks
        [HttpPost]
        public async Task<ActionResult<TemuLinkDto>> PostTemuLink(CreateTemuLinkDto createDto)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var link = await _temuLinkService.CreateLinkAsync(createDto, userId);
            return CreatedAtAction("GetTemuLink", new { id = link.Id }, link);
        }

        // GET: api/temulinks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TemuLinkDto>> GetTemuLink(int id)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var link = await _temuLinkService.GetLinkByIdAsync(id, userId);
            
            if (link == null)
            {
                return NotFound();
            }

            return link;
        }

        // PUT: api/temulinks/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TemuLinkDto>> PutTemuLink(int id, UpdateTemuLinkDto updateDto)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var link = await _temuLinkService.UpdateLinkAsync(id, updateDto, userId);
            
            if (link == null)
            {
                return NotFound();
            }

            return Ok(link);
        }

        // DELETE: api/temulinks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTemuLink(int id)
        {
            var userId = (int)HttpContext.Items["UserId"]!;
            var deleted = await _temuLinkService.DeleteLinkAsync(id, userId);
            
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
