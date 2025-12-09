using Microsoft.AspNetCore.Mvc;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class MapController(
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory,
        ILogger<MapController> logger) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly IHttpClientFactory _httpClientFactory = httpClientFactory;
        private readonly ILogger<MapController> _logger = logger;

        [HttpGet("tiles/{theme}/{z}/{x}/{y}.png")]
        [ResponseCache(Duration = 86400)]
        public async Task<IActionResult> GetTile(
            string theme,
            int z,
            int x,
            int y)
        {
            if (theme != "light" && theme != "dark")
            {
                return BadRequest("Invalid theme. Use 'light' or 'dark'");
            }

            if (z < 0 || z > 18 || x < 0 || y < 0)
            {
                return BadRequest("Invalid tile coordinates");
            }

            var token = _configuration["JAWG_KEY"] ??
                _configuration["ExternalApis:JawgKey"] ??
                throw new InvalidOperationException("Couldnt get Jawg Key!");

            if (string.IsNullOrEmpty(token))
            {
                return StatusCode(500, "Map service not configured");
            }

            var style = theme == "dark" ? "jawg-dark" : "jawg-lagoon";

            var url = $"https://tile.jawg.io/{style}/{z}/{x}/{y}.png?access-token={token}";

            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode);
                }

                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                return File(imageBytes, "image/png");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching map tile: {ex.Message}");
                return StatusCode(500, "Error fetching map tile");
            }
        }
    }
}
