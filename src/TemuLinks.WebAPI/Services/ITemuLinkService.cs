using TemuLinks.DAL.Entities;
using TemuLinks.WebAPI.DTOs;

namespace TemuLinks.WebAPI.Services
{
    public interface ITemuLinkService
    {
        Task<IEnumerable<TemuLinkDto>> GetUserLinksAsync(int userId);
        Task<IEnumerable<TemuLinkDto>> GetPublicLinksAsync();
        Task<TemuLinkDto?> GetLinkByIdAsync(int id, int userId);
        Task<TemuLinkDto> CreateLinkAsync(CreateTemuLinkDto createDto, int userId);
        Task<TemuLinkDto?> UpdateLinkAsync(int id, UpdateTemuLinkDto updateDto, int userId);
        Task<bool> DeleteLinkAsync(int id, int userId);
        Task<int> GetUserLinkCountAsync(int userId);
    }
}
