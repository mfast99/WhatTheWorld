using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Application.Services;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=countries.db"));

builder.Services.AddMemoryCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
