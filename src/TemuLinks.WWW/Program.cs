using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TemuLinks.WWW;
using TemuLinks.WWW.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add HttpClient for API communication
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5002") });

// Add Authentication Services
builder.Services.AddScoped<IAuthService, AuthService>();

var host = builder.Build();

// Health-Check beim Start (non-blocking)
_ = Task.Run(async () =>
{
    try
    {
        var http = host.Services.GetRequiredService<HttpClient>();
        var response = await http.GetAsync("api/health");
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"[WWW Health] {(int)response.StatusCode} {response.ReasonPhrase} - {content}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[WWW Health] error: {ex.Message}");
    }
});

await host.RunAsync();
