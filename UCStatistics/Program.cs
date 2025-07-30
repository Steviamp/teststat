using UCStatistics.Components;
using Microsoft.AspNetCore.Authentication.Negotiate;
using UCStatistics.Data;
using UCStatistics.Repositories;
using UCStatistics.Services;
using MudBlazor.Services;
using MudBlazor;
using UCStatistics.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions(options =>
    {
        options.DetailedErrors = true;
    });

// Add MudBlazor services
builder.Services.AddMudServices();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
});


// Configure Active Directory settings
var activeDirectorySettings = builder.Configuration
    .GetSection("ActiveDirectorySettings")
    .Get<ActiveDirectorySettings>();
builder.Services.AddSingleton(activeDirectorySettings!);

// Add authentication
if (OperatingSystem.IsWindows())
{
    builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
       .AddNegotiate();

    builder.Services.AddAuthorization(options =>
    {
        // By default, all incoming requests will be authorized according to the default policy.
        options.FallbackPolicy = options.DefaultPolicy;
    });
}

// Add custom services
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<ExcelExportService>();

// Add AuthenticationService only on Windows
if (OperatingSystem.IsWindows())
{
    builder.Services.AddScoped<AuthenticationService>();
}

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .RequireAuthorization(); // Require authentication for all pages

app.Run();
