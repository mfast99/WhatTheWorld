using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services
{
    public sealed class PerplexityService(
        HttpClient httpClient,
        IConfiguration config,
        ICountryRepository countryRepository,
        ILogger<PerplexityService> logger) : IPerplexityService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _config = config;
        private readonly string _apiKey = config["PERPLEXITY_API_KEY"] ??
            config["ExternalApis:PerplexityApiKey"] ??
            throw new InvalidOperationException("PerplexityApiKey missing");
        private readonly ICountryRepository _countryRepository = countryRepository;
        private readonly ILogger<PerplexityService> _logger = logger;

        public async Task<List<NewsDto>> GenerateNewsByCountryIdAsync(int countryId)
        {
            try
            {
                var country = await _countryRepository.GetCountryByIdAsync(countryId);
                var promptTemplate = _config["PERPLEXITY_NEWS_PROMPT"] ?? 
                    _config["PerplexityNewsPrompt"] ??
                    throw new InvalidOperationException("Couldnt get Perplexity Prompt!");

                var prompt = promptTemplate
                    .Replace("{{COUNTRY_NAME}}", country!.Name)
                    .Replace("{{COUNTRY_CODE}}", country.Code);

                var requestBody = new
                {
                    model = "sonar",
                    messages = new[] { new { role = "user", content = prompt } },
                    max_tokens = 512,
                    temperature = 0.05
                };

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.PostAsJsonAsync("", requestBody);
                response.EnsureSuccessStatusCode();

                var responseJson = await response.Content.ReadAsStringAsync();
                var parsedResponse = JsonDocument.Parse(responseJson);
                var content = parsedResponse.RootElement
                    .GetProperty("choices")[0]
                    .GetProperty("message")
                    .GetProperty("content")
                    .GetString() ?? "[]";

                var newsArray = JsonDocument.Parse(content).RootElement;
                var newsList = new List<NewsDto>();

                foreach (var item in newsArray.EnumerateArray())
                {
                    if (item.ValueKind == JsonValueKind.Null) continue;

                    var newsDto = new NewsDto(
                        0,
                        item.TryGetProperty("publishedAt", out var pubDate)
                            ? DateTime.Parse(pubDate.GetString() ?? "")
                            : DateTime.UtcNow,
                        DateTime.UtcNow,
                        item.GetProperty("title").GetString() ?? "",
                        item.GetProperty("url").GetString() ?? "",
                        item.GetProperty("source").GetString() ?? "Unknown",
                        item.GetProperty("summary").GetString() ?? ""
                    );
                    newsList.Add(newsDto);
                }

                newsList = [.. newsList.Take(3)];
                return newsList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Perplexity Error for CountryId {CountryId}", countryId);
                return [];
            }
        }
    }
}
