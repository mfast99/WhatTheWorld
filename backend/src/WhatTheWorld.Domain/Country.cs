
namespace WhatTheWorld.Domain
{
    public sealed record CountryDto(
        int Id,
        string Code,
        string Name,
        string Capital,
        string FlagEmoji,
        double Lat,
        double Lon,
        string Region,
        string Subregion,
        string Population,
        string AreaKm2,
        string Timezones,
        string Currencies,
        string Languages
    );

    public sealed record CountryListDto(
        int Id,
        string Name,
        double Lat,
        double Lon
    );

    public class CountryEntity
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Capital { get; set; } = string.Empty;
        public string FlagEmoji { get; set; } = string.Empty;
        public double Lat { get; set; }
        public double Lon { get; set; }
        public string Region { get; set; } = string.Empty;
        public string Subregion { get; set; } = string.Empty;
        public string Population { get; set; } = string.Empty;
        public string AreaKm2 { get; set; } = string.Empty;
        public string Timezones { get; set; } = string.Empty;
        public string Currencies { get; set; } = string.Empty;
        public string Languages { get; set; } = string.Empty;
        public List<NewsEntity> News { get; set; } = [];
        public List<WeatherEntity> Weather { get; set; } = [];
    }
}
