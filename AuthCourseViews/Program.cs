using AuthCourseViews;
using AuthCourseViews.Services.Auth;
using AuthCourseViews.Services.Repository;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//Añadimos un singleton de la clase HttpClient. Esta se usará para hacer peticiones a la API.
builder.Services.AddSingleton<HttpClient>(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7129"), Timeout = TimeSpan.FromMinutes(1) });

builder.Services.AddScoped<AuthProviderJWT>();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, AuthProviderJWT>(provider => provider.GetRequiredService<AuthProviderJWT>());

builder.Services.AddScoped<ILoginService, AuthProviderJWT>(provider => provider.GetRequiredService<AuthProviderJWT>());

//Añadimos un servicio que implemente la interfáz del patrón repositorio.
builder.Services.AddScoped<IDataRepository, DefaultRepositoryService>();

builder.Services.AddSweetAlert2();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();