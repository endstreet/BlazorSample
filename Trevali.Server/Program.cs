using GrapeCity.ActiveReports.Aspnetcore.Viewer;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Trevali.Server.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseReporting(settings =>
{
    var reportsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
    settings.UseFileStore(new DirectoryInfo(reportsFolder));
    settings.UseCompression = true;
});

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
