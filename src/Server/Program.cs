using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;

using Microsoft.Extensions.FileProviders;
using MudBlazor.Services;
using QYBlog.Shared.Utils;
using System.Net;

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

app.UseMiddleware<AllowedExtensionsMiddleware>();


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "content\\post")),
    RequestPath = "/content/post",
});

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

public class AllowedExtensionsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] files = new string[] { ".jpg", ".jpeg", ".png", ".gif" };
    public AllowedExtensionsMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var fileExtension = Path.GetExtension(context.Request.Path);
        if (!string.IsNullOrEmpty(fileExtension) && !files.Contains(fileExtension))
        {
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return;
        }

        await _next(context);
    }
}