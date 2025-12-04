using WhatTheWorld.Domain;

namespace WhatTheWorld.Infrastructure.Mappings
{
    public static class CountryExtensions
    {
        public static CountryDto? ToDto(this CountryEntity? country)
        {
            if (country == null)
                return null;

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
    }
}
