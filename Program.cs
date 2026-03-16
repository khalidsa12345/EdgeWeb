var builder = WebApplication.CreateBuilder(args);

// 👇 Tell ASP.NET to use the Render port
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Optional: you can remove this on Render to avoid warning
// app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();