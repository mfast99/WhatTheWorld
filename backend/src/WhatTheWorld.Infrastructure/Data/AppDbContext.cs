using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Country> Countries { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Code);
            entity.Property(e => e.Code).HasMaxLength(2);
            entity.HasIndex(e => e.Name);
        });

        modelBuilder.Entity<Country>().HasData(
            new Country("DE", "Germany", "Berlin", "🇩🇪", 52.52, 13.405),
            new Country("US", "United States", "Washington D.C.", "🇺🇸", 38.8951, -77.0364),
            new Country("FR", "France", "Paris", "🇫🇷", 48.8566, 2.3522),
            new Country("GB", "United Kingdom", "London", "🇬🇧", 51.5074, -0.1278)
        );
    }
}
