using Microsoft.AspNetCore.Mvc;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class MapController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public MapController(
            IConfiguration configuration,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

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

            var token = _configuration["JawgKey"];
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
                Console.WriteLine($"Error fetching map tile: {ex.Message}");
                return StatusCode(500, "Error fetching map tile");
            }
        }
    }
}
