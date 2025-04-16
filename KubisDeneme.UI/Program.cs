using KubisDeneme.UI.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using KubisDeneme.Api;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// HttpClient servisini ekleyin (API'nizin adresini belirtin)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7241/") });

builder.Services.AddHttpClient("KubisApi", client =>
{
    client.BaseAddress = new Uri("https://localhost:7241/"); // API'nizin adresi
});


builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRazorPages();
builder.Services.AddBlazorBootstrap();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
