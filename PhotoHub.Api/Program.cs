
using Microsoft.AspNetCore.Http.Features;
using PhotoHub.Api.Storage; // <-- we'll add this namespace below

var builder = WebApplication.CreateBuilder(args);

// CORS — allow React on port 3000 (and a Codespaces URL via config/env)
var frontendUrls = builder.Configuration
    .GetSection("FrontendUrls")
    //.Get<string[]>() ?? new[] { "http://localhost:3000" };
    .Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Frontend", policy =>
    {
        policy.WithOrigins(frontendUrls)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

// Allow up to 20 MB images
builder.Services.Configure<FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 20 * 1024 * 1024; // 20 MB
});

builder.Services.AddControllers();

// Storage backend: Local by default; we’ll switch to Azure later via config.
builder.Services.AddSingleton<IPhotoStorage, LocalPhotoStorage>();

var app = builder.Build();

// In Codespaces, ensure Kestrel listens on 0.0.0.0:5000.
// If needed, run with: dotnet run --urls=http://0.0.0.0:5000

app.UseCors("Frontend");

// Serve static files from wwwroot (so /uploads/* works)
app.UseStaticFiles();

app.MapControllers();

app.Run();

