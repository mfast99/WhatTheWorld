using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Api;
using WhatTheWorld.Application.Services;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<INewsService, NewsService>();
builder.Services.AddScoped<IWeatherService, WeatherService>();
builder.Services.AddScoped<IPerplexityService, PerplexityService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=countries.db"));

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<IWeatherService, WeatherService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalApis:WeatherApiBaseUrl"]
        ?? "https://api.weatherapi.com/v1/current.json");
    client.DefaultRequestHeaders.Add("User-Agent", "WhatTheWorld/1.0");
});
builder.Services.AddHttpClient<IPerplexityService, PerplexityService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["ExternalApis:PerplexityBaseUrl"]
        ?? "https://api.perplexity.ai/chat/completions");
    client.DefaultRequestHeaders.Add("User-Agent", "WhatTheWorld/1.0");
});
builder.Services.AddHttpClient<CountrySeedService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", builder =>
    {
        builder
            .WithOrigins("http://localhost:5173", "http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var seedService = scope.ServiceProvider.GetRequiredService<CountrySeedService>();

    try
    {
        Console.WriteLine("Clearing existing countries...");
        context.Countries.RemoveRange(context.Countries);
        await context.SaveChangesAsync();
        Console.WriteLine("Countries cleared!");

        Console.WriteLine("Seeding countries...");
        await seedService.SeedCountriesAsync(context);
        Console.WriteLine("Seeding complete!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error: {ex.Message}");
        throw;
    }
}

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
