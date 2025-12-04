using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services.Interfaces
{
    public class PerplexityService : IPerplexityService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IMemoryCache _cache;
        private readonly string _apiKey;
        private readonly string _prompt;
        private readonly INewsRepository _newsRepository;

        public PerplexityService(
            HttpClient httpClient,
            IConfiguration config,
            IMemoryCache cache,
            INewsRepository newsRepository)
        {
            _httpClient = httpClient;
            _config = config;
            _cache = cache;
            _apiKey = config["PerplexityApiKey"] ?? 
                throw new InvalidOperationException("PerplexityApiKey missing");
            _prompt = config["PerplexityNewsPrompt"] ?? 
                throw new InvalidOperationException("PerplexityNewsPrompt missing");
            _newsRepository = newsRepository;
        }
    }
}
