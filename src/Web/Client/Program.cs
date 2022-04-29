using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QueryModel;
using Syncfusion.Blazor;
using Web.Client;
using Web.Client.Data;
using Web.Client.Pages;
using Web.Client.RPC;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Web.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Web.ServerAPI"));

// configure other dependencies
builder.Services.AddScoped<IQueryService, QueryServiceProxy>();
builder.Services.AddScoped<IQueryDb, RemoteQueryDbContext>();
builder.Services.AddScoped<CustomAdaptor>();

builder.Services.AddSyncfusionBlazor(options => { options.IgnoreScriptIsolation = false; });

await builder.Build().RunAsync();
