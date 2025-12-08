


/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
 
*/


using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1) Register CORS
var allowedOrigins = new[]
{
    // Replace with your React dev origins:
    "http://localhost:3000",           // CRA default
    "http://localhost:5173",           // Vite default
    "https://symmetrical-pancake-9jpxv7rq74pcxw5j-3000.app.github.dev", // Codespaces (adjust port)
    "https://symmetrical-pancake-9jpxv7rq74pcxw5j-5173.app.github.dev"  // Vite in Codespaces
    // Or use your deployed frontend origin(s)
};

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyMethod()
              .AllowAnyHeader();
        // If you need cookies/Authorization header across origins:
        // policy.AllowCredentials();
    });

    // For quick local troubleshooting (dev only!)
    // options.AddPolicy("AllowAll", p => p
    //     .AllowAnyOrigin()
    //     .AllowAnyMethod()
    //     .AllowAnyHeader());
});

builder.Services.AddControllers(); // or minimal APIs

var app = builder.Build();
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();



// 2) Enable CORS in the pipeline
app.UseHttpsRedirection();

app.UseRouting();

// Choose ONE policy to use globally:
app.UseCors("FrontendPolicy"); // or "AllowAll" for dev only

app.UseAuthorization();

app.MapControllers();
// For minimal APIs, attach per‑endpoint if preferred:
// app.MapGet("/weatherforecast", () => ...).RequireCors("FrontendPolicy");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}