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

await builder.Build().RunAsync();
