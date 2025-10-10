using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TemuLinks.WWW;
using TemuLinks.WWW.Services;
using MudBlazor.Services;
using TemuLinks.WWW.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Bind ApiOptions from appsettings.json and configure HttpClient BaseAddress accordingly
var apiOptions = new ApiOptions();
builder.Configuration.Bind("Api", apiOptions);
var baseUrl = string.IsNullOrWhiteSpace(apiOptions.BaseUrl) ? "http://localhost:5909/" : apiOptions.BaseUrl;
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseUrl!) });

// Add Authentication Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITemuLinksApiClient, TemuLinksApiClient>();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
