using TemuLinks.Web.Components;
using TemuLinks.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Entity Framework
builder.Services.AddDbContext<TemuLinksDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 0))));

// Microsoft Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
})
.AddCookie()
.AddMicrosoftAccount(options =>
{
    options.ClientId = builder.Configuration["MicrosoftAccount:ClientId"] ?? "";
    options.ClientSecret = builder.Configuration["MicrosoftAccount:ClientSecret"] ?? "";
});

// Authorization
builder.Services.AddAuthorization();

// HttpClient für API-Aufrufe
builder.Services.AddHttpClient("TemuLinksAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/api");
});

// Services
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<TemuLinks.Web.Services.ITemuLinksApiService, TemuLinks.Web.Services.TemuLinksApiService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

// app.UseHttpsRedirection(); // Disabled for development
app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
