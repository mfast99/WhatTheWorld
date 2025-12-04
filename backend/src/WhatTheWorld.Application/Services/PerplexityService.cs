using Microsoft.Extensions.Configuration;
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
        ICountryRepository countryRepository) : IPerplexityService
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly IConfiguration _config = config;
        private readonly string _apiKey = config["PerplexityApiKey"] ??
                throw new InvalidOperationException("PerplexityApiKey missing");
        private readonly ICountryRepository _countryRepository = countryRepository;

        public async Task<List<NewsDto>> GenerateNewsByCountryIdAsync(int countryId)
        {
            try
            {
                var country = await _countryRepository.GetCountryByIdAsync(countryId);
                var promptTemplate = _config["PerplexityNewsPrompt"] ?? throw new Exception();
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
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

                var response = await _httpClient.PostAsJsonAsync("https://api.perplexity.ai/chat/completions", requestBody);
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
                Console.WriteLine($"Perplexity Error: {ex.Message}");
                return [];
            }
        }
    }
}
