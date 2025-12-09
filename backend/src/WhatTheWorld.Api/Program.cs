using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using AspNetCoreRateLimit;
using Serilog;
using Serilog.Events;
using WhatTheWorld.Api;
using WhatTheWorld.Application.Services;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

// Logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("/var/log/whattheworld.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseSerilog();
    builder.Logging.AddSerilog();

    builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        });

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                               ?? "Data Source=whattheworld.db";
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(connectionString));

    builder.Services.AddMemoryCache();

    builder.Services.AddScoped<ICountryRepository, CountryRepository>();
    builder.Services.AddScoped<INewsRepository, NewsRepository>();
    builder.Services.AddScoped<IWeatherRepository, WeatherRepository>();
    builder.Services.AddScoped<ICountryService, CountryService>();
    builder.Services.AddScoped<INewsService, NewsService>();
    builder.Services.AddScoped<IWeatherService, WeatherService>();
    builder.Services.AddScoped<IPerplexityService, PerplexityService>();

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

    builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
    builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
    }

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowFrontend", policy =>
        {
            policy
                .WithOrigins(
                    "https://mfast47.de",
                    "http://localhost:5173",
                    "http://localhost:4173"
                )
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var context = serviceProvider.GetRequiredService<AppDbContext>();
        var seedService = serviceProvider.GetRequiredService<CountrySeedService>();

        try
        {
            Log.Information("Applying database migrations...");
            await context.Database.MigrateAsync();
            Log.Information("Database migrations applied successfully.");

            if (!await context.Countries.AnyAsync())
            {
                Log.Information("Country seeding started");
                await seedService.SeedCountriesAsync(context);
                Log.Information("Country seeding completed");
            }
            else
            {
                Log.Information("Countries exist, skip seeding");
            }
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly during database initialization (Migration or Seeding).");
            throw;
        }
    }

    app.UseForwardedHeaders(new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
    });

    app.UseCors("AllowFrontend");
    app.UseIpRateLimiting();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();

    Log.Information("WhatTheWorld API starting in {Environment}", app.Environment.EnvironmentName);
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}