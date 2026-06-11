using EnterpriseServer.Data;
using EnterpriseServer.Services;
using Microsoft.EntityFrameworkCore;
using Polly;
using Polly.Extensions.Http;

var builder =
    WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration
            .GetConnectionString("Default")));

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddHttpClient("WeatherApi", client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["WeatherApi:BaseUrl"]
            ?? "https://api.example.com");
        
        client.Timeout = TimeSpan.FromSeconds(30);
    })
    .AddPolicyHandler(
        HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))));

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

// For production use db.Database.Migrate()
// with proper EF Core migrations instead.
// EnsureCreated is used here for simplicity
// since this sample has no migration files.
using (var scope =
    app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    
    db.Database.EnsureCreated();
}

app.UseMiddleware<ApiKeyMiddleware>();
app.MapMcp();

app.Run();
