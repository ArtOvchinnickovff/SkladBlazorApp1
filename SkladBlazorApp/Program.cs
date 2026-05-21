using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using SkladBlazorApp;
using SkladBlazorApp.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<AuthMessageHandler>();

// Настраиваем клиент так, чтобы он автоматически слал запросы на текущий домен сайта
builder.Services.AddHttpClient("AuthClient", client =>
{
    
    client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress);
})
.AddHttpMessageHandler<AuthMessageHandler>();

// Регистрация HttpClient по умолчанию
builder.Services.AddScoped(sp =>
{
    return sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthClient");
});

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();