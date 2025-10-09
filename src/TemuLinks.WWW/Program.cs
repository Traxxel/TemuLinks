using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using TemuLinks.WWW;
using TemuLinks.WWW.Options;
using TemuLinks.WWW.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure API options from appsettings.json
builder.Services.Configure<ApiOptions>(builder.Configuration.GetSection("Api"));

// Add HttpClient for API communication using configured base URL
builder.Services.AddScoped(sp =>
{
    var apiOptions = sp.GetRequiredService<IOptions<ApiOptions>>().Value;
    var baseUrl = string.IsNullOrWhiteSpace(apiOptions.BaseUrl)
        ? builder.HostEnvironment.BaseAddress
        : apiOptions.BaseUrl;
    return new HttpClient { BaseAddress = new Uri(baseUrl) };
});

// Add Authentication Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITemuLinksApiClient, TemuLinksApiClient>();

await builder.Build().RunAsync();
