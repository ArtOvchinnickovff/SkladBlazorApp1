using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

using SkladBlazorApp;
using SkladBlazorApp.Client.Services;




var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");




builder.Services.AddScoped(sp =>
{
    return sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthClient");
});

builder.Services.AddTransient<AuthMessageHandler>();  // Transient для handler
builder.Services.AddHttpClient("AuthClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5121");
})
.AddHttpMessageHandler<AuthMessageHandler>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();  // Один раз

await builder.Build().RunAsync();