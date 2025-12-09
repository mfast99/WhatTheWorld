using Microsoft.AspNetCore.Mvc;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class NewsController(INewsService newsService) : ControllerBase
    {
        private readonly INewsService _newsService = newsService;

        /// <summary>
        /// Get cached news
        /// </summary>
        [HttpGet("cached")]
        public async Task<IActionResult> GetCachedNews([FromQuery] int countryId)
        {
            var news = await _newsService.GetCachedNewsAsync(countryId);
            return Ok(news);
        }

        /// <summary>
        /// Refresh news if needed
        /// Returns 304 Not Modified if already fresh
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshNews([FromQuery] int countryId)
        {
            var result = await _newsService.RefreshNewsAsync(countryId);

            if (!result.WasRefreshed)
            {
                // Return 304 with info when next refresh is allowed
                Response.Headers.Append("X-Next-Refresh", result.NextFetchAllowed.ToString("o"));
                return StatusCode(304);  // Not Modified
            }

            // Return 200 with fresh news
            return Ok(result);
        }

        /// <summary>
        /// Legacy endpoint
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNews([FromQuery] int countryId)
        {
            var cached = await _newsService.GetCachedNewsAsync(countryId);

            if (cached.Count > 0)
            {
                return Ok(cached);
            }

            var result = await _newsService.RefreshNewsAsync(countryId);
            return Ok(result.News);
        }
    }
}
