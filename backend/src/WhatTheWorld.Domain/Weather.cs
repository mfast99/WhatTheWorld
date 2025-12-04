namespace WhatTheWorld.Domain
{
    public record WeatherDto(
        int Id,
        DateTime Time,
        double TempCelsius,
        double TempFahrenheit,
        string Description, 
        string IconUrl
    );

    public class WeatherEntity
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public DateTime Time { get; set; }
        public double TempCelsius { get; set; }
        public double TempFahrenheit { get; set; }
        public string Description { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;

        // Navigation Property
        public CountryEntity Country { get; set; } = new();
    }
}
