using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Mappings
{
    public static class CountryExtensions
    {
        public static CountryDto ToDto(this CountryEntity country)
        {
            return new CountryDto(
                country.Id,
                country.Code,
                country.Name,
                country.Capital,
                country.FlagEmoji,
                country.Lat,
                country.Lon,
                country.Region,
                country.Subregion,
                country.Population,
                country.AreaKm2,
                country.Timezones,
                country.Currencies,
                country.Languages
            );
        }

        public static CountryListDto ToListDto(this CountryEntity country)
        {
            return new CountryListDto(
                country.Id,
                country.Name,
                country.Lat,
                country.Lon
            );
        }
    }
}
