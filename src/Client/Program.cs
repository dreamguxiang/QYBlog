using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { 
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
});

builder.Services.AddMudServices();

var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IMudPopoverService));
if (descriptor != null)
{
    builder.Services.Remove(descriptor);
}
builder.Services.AddMudPopoverService(delegate (PopoverOptions o)
{
    o.ThrowOnDuplicateProvider = false;
});

await builder.Build().RunAsync();

