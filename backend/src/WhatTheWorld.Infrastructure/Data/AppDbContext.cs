using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<CountryEntity> Countries { get; set; }
    public DbSet<NewsEntity> News { get; set; }
    public DbSet<WeatherEntity> Weather { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CountryEntity>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();
            entity.Property(c => c.Code).HasMaxLength(2).IsRequired();
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Population).HasMaxLength(20);
            entity.Property(c => c.AreaKm2).HasMaxLength(20);
            entity.HasIndex(c => c.Code).IsUnique();
        });

        modelBuilder.Entity<NewsEntity>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Title).HasMaxLength(500).IsRequired();
            entity.Property(n => n.Url).HasMaxLength(1000).IsRequired();
            entity.Property(n => n.Source).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.HasOne(n => n.Country)
                  .WithMany(c => c.News)
                  .HasForeignKey(n => n.CountryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WeatherEntity>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Description).HasMaxLength(200);
            entity.Property(w => w.IconUrl).HasMaxLength(500);
            entity.Property(c => c.Id).ValueGeneratedOnAdd();

            entity.HasOne(w => w.Country)
                  .WithMany(c => c.Weather)
                  .HasForeignKey(w => w.CountryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
