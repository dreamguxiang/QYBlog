using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.Extensions.FileProviders;
using MudBlazor;
using MudBlazor.Services;
using QYBlog.Shared.Utils;

Utils.CreateDirectory();

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//Server模式
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();


//////////////////////////////////////////////////////////////////////////////////
//移除IMudPopoverService服务,并重新注册,以关闭ThrowOnDuplicateProvider （临时处理）
var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IMudPopoverService));
if (descriptor != null)
{
    builder.Services.Remove(descriptor);
}

builder.Services.AddMudPopoverService(delegate (PopoverOptions o)
{
    o.ThrowOnDuplicateProvider = false;
});
//////////////////////////////////////////////////////////////////////////////////

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseWebAssemblyDebugging();
}
app.UseBlazorFrameworkFiles();

//////////////////静态文件注册/////////////////
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "content/post")),
    RequestPath = "/content/post",
});
/////////////////////////////////////////////


app.UseRouting();

app.MapRazorPages();
app.MapControllers();

//Server模式
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();



/// <summary>
/// 额外服务
/// </summary>
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

