using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using QYBlog.Shared.Utils;
using System.Reflection.PortableExecutable;

Utils.CreateDirectory();

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Server模式
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

//注册API服务
builder.Services.AddControllers();
builder.Services.AddCors();

if (!builder.Services.Any(x => x.ServiceType == typeof(HttpClient)))
{
    builder.Services.AddScoped(sp =>
    {
        var uriHelper = sp.GetRequiredService<NavigationManager>();
        return new HttpClient
        {
            BaseAddress = new Uri(uriHelper.BaseUri)
        };
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();

//Server模式
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();


public enum HybridType
{
    ServerSide,
    WebAssembly,
    HybridManual,
    HybridOnNavigation,
    HybridOnReady
}

public class HybridOptions
{
    public HybridType HybridType { get; set; }
}
