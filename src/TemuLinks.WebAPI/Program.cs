using Microsoft.EntityFrameworkCore;
using TemuLinks.DAL;
using Pomelo.EntityFrameworkCore.MySql;
using TemuLinks.WebAPI.Services;
using TemuLinks.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework
builder.Services.AddDbContext<TemuLinksDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), 
        new MySqlServerVersion(new Version(8, 0, 21))));

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Disabled for development
app.UseCors("AllowAll");
app.UseApiKeyAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
