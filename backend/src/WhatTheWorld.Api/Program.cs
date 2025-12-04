using Microsoft.EntityFrameworkCore;
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
    client.BaseAddress = new Uri("https://api.weatherapi.com/v1/");
    client.DefaultRequestHeaders.Add("User-Agent", "WhatTheWorld/1.0");
});
builder.Services.AddHttpClient<IPerplexityService, PerplexityService>(client =>
{
    client.BaseAddress = new Uri("https://api.perplexity.ai/");
    client.DefaultRequestHeaders.Add("User-Agent", "WhatTheWorld/1.0");
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
