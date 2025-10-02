using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;
using TemuLinks.DAL.Entities;
using TemuLinks.WebAPI.DTOs;

namespace TemuLinks.WebAPI.Services
{
    public class TemuLinkService : ITemuLinkService
    {
        private readonly TemuLinksDbContext _context;

        public TemuLinkService(TemuLinksDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TemuLinkDto>> GetUserLinksAsync(int userId)
        {
            var links = await _context.TemuLinks
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new TemuLinkDto
                {
                    Id = l.Id,
                    Url = l.Url,
                    Description = l.Description,
                    IsPublic = l.IsPublic,
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();

            return links;
        }

        public async Task<IEnumerable<TemuLinkDto>> GetPublicLinksAsync()
        {
            var links = await _context.TemuLinks
                .Include(l => l.User)
                .Where(l => l.IsPublic)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new TemuLinkDto
                {
                    Id = l.Id,
                    Url = l.Url,
                    Description = l.Description,
                    IsPublic = l.IsPublic,
                    CreatedAt = l.CreatedAt,
                    UserName = l.User.FirstName + " " + l.User.LastName
                })
                .ToListAsync();

            return links;
        }

        public async Task<TemuLinkDto?> GetLinkByIdAsync(int id, int userId)
        {
            var link = await _context.TemuLinks
                .Where(l => l.Id == id && l.UserId == userId)
                .Select(l => new TemuLinkDto
                {
                    Id = l.Id,
                    Url = l.Url,
                    Description = l.Description,
                    IsPublic = l.IsPublic,
                    CreatedAt = l.CreatedAt
                })
                .FirstOrDefaultAsync();

            return link;
        }

        public async Task<TemuLinkDto> CreateLinkAsync(CreateTemuLinkDto createDto, int userId)
        {
            var link = new TemuLink
            {
                UserId = userId,
                Url = createDto.Url,
                Description = createDto.Description,
                IsPublic = createDto.IsPublic,
                CreatedAt = DateTime.UtcNow
            };

            _context.TemuLinks.Add(link);
            await _context.SaveChangesAsync();

            return new TemuLinkDto
            {
                Id = link.Id,
                Url = link.Url,
                Description = link.Description,
                IsPublic = link.IsPublic,
                CreatedAt = link.CreatedAt
            };
        }

        public async Task<TemuLinkDto?> UpdateLinkAsync(int id, UpdateTemuLinkDto updateDto, int userId)
        {
            var link = await _context.TemuLinks
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);

            if (link == null)
                return null;

            link.Url = updateDto.Url;
            link.Description = updateDto.Description;
            link.IsPublic = updateDto.IsPublic;

            await _context.SaveChangesAsync();

            return new TemuLinkDto
            {
                Id = link.Id,
                Url = link.Url,
                Description = link.Description,
                IsPublic = link.IsPublic,
                CreatedAt = link.CreatedAt
            };
        }

        public async Task<bool> DeleteLinkAsync(int id, int userId)
        {
            var link = await _context.TemuLinks
                .FirstOrDefaultAsync(l => l.Id == id && l.UserId == userId);

            if (link == null)
                return false;

            _context.TemuLinks.Remove(link);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<int> GetUserLinkCountAsync(int userId)
        {
            return await _context.TemuLinks
                .CountAsync(l => l.UserId == userId);
        }
    }
}
