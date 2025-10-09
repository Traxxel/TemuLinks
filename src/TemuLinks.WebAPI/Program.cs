using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TemuLinks.DAL;
using TemuLinks.WebAPI.Services;
using TemuLinks.WebAPI.Middleware;
using TemuLinks.DAL.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT Auth
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection.GetValue<string>("Key") ?? "";
var keyBytes = Encoding.UTF8.GetBytes(jwtKey);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection.GetValue<string>("Issuer"),
        ValidAudience = jwtSection.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
    };
});

// Add Entity Framework
builder.Services.AddDbContext<TemuLinksDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Services
builder.Services.AddScoped<ITemuLinkService, TemuLinkService>();
builder.Services.AddScoped<IApiKeyService, ApiKeyService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// DB reachability check and initial admin seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = app.Logger;
    try
    {
        var db = services.GetRequiredService<TemuLinksDbContext>();
        var canConnect = db.Database.CanConnect();
        if (!canConnect)
        {
            logger.LogError("[Startup] Database is NOT reachable. Check connection string and server availability.");
        }
        else
        {
            if (!db.Users.Any())
            {
                logger.LogInformation("[Startup] No users found. Seeding initial admin user.");
                var admin = new User
                {
                    Username = "admin",
                    Email = "admin@local",
                    PasswordHash = PasswordHasher.HashPassword("admin"),
                    FirstName = "Admin",
                    LastName = "User",
                    IsActive = true,
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                };
                db.Users.Add(admin);
                db.SaveChanges();
                logger.LogInformation("[Startup] Admin user created (username: 'admin').");
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "[Startup] Failed to check database connectivity or seed admin user.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for development
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseApiKeyAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
