using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;
using TemuLinks.WebAPI.Services;

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

        [HttpPost("login")]
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
                new Claim(JwtRegisteredClaimNames.Email, string.IsNullOrWhiteSpace(user.Email) ? $"{user.Username}@temulinks.local" : user.Email),
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

        
    }
}


