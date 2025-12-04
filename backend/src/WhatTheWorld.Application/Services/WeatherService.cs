using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Mappings;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;
        public WeatherService(
            IWeatherRepository weatherRepository,
            ICountryRepository countryRepository,
            HttpClient httpClient,
            IConfiguration config,
            IMemoryCache cache)
        {
            _weatherRepository = weatherRepository;
            _countryRepository = countryRepository;
            _httpClient = httpClient;
            _config = config;
            _cache = cache;
            _apiKey = config["WeatherApiKey"] ?? throw new InvalidOperationException("WeatherApiKey missing");
        }

        public async Task<WeatherDto?> GetCurrentWeatherByCountryAsync(int countryId)
        {
            var cacheKey = $"weather_{countryId}";
            if (_cache.TryGetValue(cacheKey, out WeatherDto? cachedWeather))
                return cachedWeather;

            try
            {
                var weather = await _weatherRepository.GetCurrentWeatherByCountryAsync(countryId);

                if (weather == null)
                {
                    string countryName = await _countryRepository.GetCountryNameByIdAsync(countryId);
                    var weatherEntity = await FetchWeatherFromApiAsync(countryId, countryName);
                    await _weatherRepository.CreateWeatherAsync(weatherEntity);
                    weather = weatherEntity.ToDto();
                }

                _cache.Set(cacheKey, weather, TimeSpan.FromMinutes(10));
                return weather;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Weather API Error: {ex.Message}");
                return null;
            }
        }

        private async Task<WeatherEntity> FetchWeatherFromApiAsync(int countryId, string countryName)
        {
            try
            {
                var url = $"current.json?key={_apiKey}&q={countryName}&aqi=no";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException();

                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                var current = root.GetProperty("current");
                var condition = current.GetProperty("condition");

                WeatherEntity weather = new WeatherEntity
                {
                    CountryId = countryId,
                    Time = DateTime.UtcNow,
                    TempCelsius = current.GetProperty("temp_c").GetDouble(),
                    TempFahrenheit = current.GetProperty("temp_f").GetDouble(),
                    Description = condition.GetProperty("text").GetString() ?? "Unknown",
                    IconUrl = $"https:{condition.GetProperty("icon").GetString()}"
                };

                return weather;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Fetch Weather Error: {ex.Message}");
                throw;
            }
        }
    }
}
}
