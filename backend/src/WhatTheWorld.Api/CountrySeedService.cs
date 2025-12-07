using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;

namespace WhatTheWorld.Api
{
    public class CountrySeedService
    {
        private readonly HttpClient _httpClient;

        public CountrySeedService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SeedCountriesAsync(AppDbContext context)
        {
            try
            {
                if (await context.Countries.AnyAsync())
                {
                    Console.WriteLine("Countryseeder: Countries already exist");
                    return;
                }

                Console.WriteLine("Loading countries from rest api...");

                var countries = await FetchCountriesFromApiAsync();

                if (countries == null || countries.Count == 0)
                {
                    Console.WriteLine("No countries loaded!");
                    return;
                }

                var countryEntities = ConvertToCountryEntities(countries);

                context.Countries.AddRange(countryEntities);
                await context.SaveChangesAsync();

                Console.WriteLine($"{countryEntities.Count} countries successfully saved!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during seeding {ex.Message}");
                throw;
            }
        }

        private async Task<List<RestCountriesDto>> FetchCountriesFromApiAsync()
        {
            try
            {
                var url1 = "https://restcountries.com/v3.1/all?fields=name,cca2,flags,latlng,region,subregion,population,area,languages,capital";

                var httpResponse1 = await _httpClient.GetAsync(url1);
                var content1 = await httpResponse1.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var response1 = JsonSerializer.Deserialize<List<RestCountriesDto>>(content1, options);

                var url2 = "https://restcountries.com/v3.1/all?fields=cca2,currencies,timezones";

                Console.WriteLine($"📡 Request 2: {url2}");
                var httpResponse2 = await _httpClient.GetAsync(url2);
                var content2 = await httpResponse2.Content.ReadAsStringAsync();

                var response2 = JsonSerializer.Deserialize<List<CountryExtraDto>>(content2, options);

                if (response1 != null && response2 != null)
                {
                    var extraLookup = response2.ToDictionary(x => x.Cca2);

                    foreach (var country in response1)
                    {
                        if (extraLookup.TryGetValue(country.Cca2, out var extra))
                        {
                            country.Currencies = extra.Currencies;
                            country.Timezones = extra.Timezones;
                        }
                    }
                }

                if (response1?.Count > 0)
                {
                    var first = response1[0];
                    Console.WriteLine($"🔍 First: {first.Cca2} - {first.Name?.Common}");
                }

                return response1 ?? [];
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                return [];
            }
        }




        private List<CountryEntity> ConvertToCountryEntities(List<RestCountriesDto> apiCountries)
        {
            var entities = new List<CountryEntity>();
            var id = 1;

            var uniqueCountries = apiCountries
                .Where(c => !string.IsNullOrEmpty(c.Cca2))
                .DistinctBy(c => c.Cca2)
                .ToList();

            Console.WriteLine($"Original: {apiCountries.Count}, After Dedup: {uniqueCountries.Count}");

            foreach (var apiCountry in uniqueCountries)
            {
                try
                {
                    var flagEmoji = ExtractFlagEmoji(apiCountry.Flags?.Alt) ?? "🌍";

                    var entity = new CountryEntity
                    {
                        Id = id++,
                        Code = apiCountry.Cca2 ?? string.Empty,
                        Name = apiCountry.Name?.Common ?? apiCountry.Name?.Official ?? "Unknown",
                        Capital = apiCountry.Capital?.FirstOrDefault() ?? "Unknown",
                        FlagEmoji = flagEmoji,
                        Lat = apiCountry.Latlng?.FirstOrDefault() ?? 0,
                        Lon = apiCountry.Latlng?.ElementAtOrDefault(1) ?? 0,
                        Region = apiCountry.Region ?? "Unknown",
                        Subregion = apiCountry.Subregion ?? "Unknown",
                        Population = (apiCountry.Population ?? 0).ToString(),
                        AreaKm2 = (apiCountry.Area ?? 0).ToString("F2"),
                        Timezones = string.Join(", ", apiCountry.Timezones ?? ["UTC"]),
                        Currencies = FormatCurrencies(apiCountry.Currencies),
                        Languages = FormatLanguages(apiCountry.Languages),
                    };

                    entities.Add(entity);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error at Seeding: {apiCountry.Name?.Common}: {ex.Message}");
                    continue;
                }
            }

            return entities;
        }

        private string? ExtractFlagEmoji(string? flagsAlt)
        {
            if (string.IsNullOrEmpty(flagsAlt)) return null;

            var emoji = new string(flagsAlt.TakeWhile(c => char.IsSurrogatePair(flagsAlt, flagsAlt.IndexOf(c))).ToArray());
            return string.IsNullOrEmpty(emoji) ? null : emoji;
        }

        private string FormatCurrencies(Dictionary<string, CurrencyDto>? currencies)
        {
            if (currencies == null || currencies.Count == 0)
                return "Unknown";

            try
            {
                var currencyList = currencies
                    .Select(c => $"{c.Value.Name} ({c.Key.ToUpper()})")
                    .ToList();

                return string.Join(", ", currencyList);
            }
            catch
            {
                return "Unknown";
            }
        }

        private string FormatLanguages(Dictionary<string, string>? languages)
        {
            if (languages == null || languages.Count == 0)
                return "Unknown";

            try
            {
                var languageList = languages
                    .Select(l => l.Value)
                    .ToList();

                return string.Join(", ", languageList);
            }
            catch
            {
                return "Unknown";
            }
        }
    }
    internal class CountryExtraDto
    {
        [JsonPropertyName("cca2")]
        public string? Cca2 { get; set; }

        [JsonPropertyName("currencies")]
        public Dictionary<string, CurrencyDto>? Currencies { get; set; }

        [JsonPropertyName("timezones")]
        public List<string>? Timezones { get; set; }
    }

    internal class RestCountriesDto
    {
        [JsonPropertyName("name")]
        public NameDto? Name { get; set; }

        [JsonPropertyName("cca2")]
        public string? Cca2 { get; set; }

        [JsonPropertyName("flags")]
        public FlagsDto? Flags { get; set; }

        [JsonPropertyName("latlng")]
        public List<double>? Latlng { get; set; }

        [JsonPropertyName("region")]
        public string? Region { get; set; }

        [JsonPropertyName("subregion")]
        public string? Subregion { get; set; }

        [JsonPropertyName("population")]
        public long? Population { get; set; }

        [JsonPropertyName("area")]
        public double? Area { get; set; }

        [JsonPropertyName("timezones")]
        public List<string>? Timezones { get; set; }

        [JsonPropertyName("currencies")]
        public Dictionary<string, CurrencyDto>? Currencies { get; set; }

        [JsonPropertyName("languages")]
        public Dictionary<string, string>? Languages { get; set; }

        [JsonPropertyName("capital")]
        public List<string>? Capital { get; set; }
    }

    internal class NameDto
    {
        [JsonPropertyName("common")]
        public string? Common { get; set; }

        [JsonPropertyName("official")]
        public string? Official { get; set; }
    }

    internal class FlagsDto  
    {
        [JsonPropertyName("png")]
        public string? Png { get; set; }

        [JsonPropertyName("svg")]
        public string? Svg { get; set; }

        [JsonPropertyName("alt")]
        public string? Alt { get; set; }
    }

    internal class CurrencyDto
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("symbol")]
        public string? Symbol { get; set; }
    }

}
