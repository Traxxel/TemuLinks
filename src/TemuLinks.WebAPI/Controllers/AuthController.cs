using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;
using TemuLinks.WebAPI.Services;
using TemuLinks.DAL.Entities;

namespace TemuLinks.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TemuLinksDbContext _db;
        private readonly IConfiguration _config;
        private readonly ILogger<AuthController> _logger;

        public AuthController(TemuLinksDbContext db, IConfiguration config, ILogger<AuthController> logger)
        {
            _db = db;
            _config = config;
            _logger = logger;
        }

        public record LoginRequest(string Username, string Password);
        public record LoginResponse(string Token);
        public record RegisterRequest(string Username, string Password, string? FirstName, string? LastName);
        public record RegisterResponse(int Id, string Username, bool IsActive);

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("[Auth] Login attempt for '{Username}'", request.Username);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.IsActive);
            if (user == null)
            {
                _logger.LogWarning("[Auth] User not found or inactive: {Username}", request.Username);
                return Unauthorized("Invalid credentials");
            }

            var pwdOk = PasswordHasher.VerifyPassword(request.Password, user.PasswordHash);
            if (!pwdOk)
            {
                _logger.LogWarning("[Auth] Password verification failed for {Username}", request.Username);
                return Unauthorized("Invalid credentials");
            }

            var jwtSection = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(new LoginResponse(tokenString));
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("[Auth] Register attempt for '{Username}'", request.Username);

            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username und Password sind erforderlich");
            }

            var normalizedUsername = request.Username.Trim();

            var exists = await _db.Users.AnyAsync(u => u.Username == normalizedUsername);
            if (exists)
            {
                return Conflict("Username bereits vergeben");
            }

            var passwordHash = PasswordHasher.HashPassword(request.Password);
            var user = new User
            {
                Username = normalizedUsername,
                Email = $"{normalizedUsername}@temulinks.local", // technische Füllung für Legacy-DB-Spalte
                PasswordHash = passwordHash,
                FirstName = string.IsNullOrWhiteSpace(request.FirstName) ? null : request.FirstName!.Trim(),
                LastName = string.IsNullOrWhiteSpace(request.LastName) ? null : request.LastName!.Trim(),
                Role = "User",
                IsActive = false
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            var response = new RegisterResponse(user.Id, user.Username, user.IsActive);
            return CreatedAtAction(nameof(Login), new { username = user.Username }, response);
        }
        
    }
}


