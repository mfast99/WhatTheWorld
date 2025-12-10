using Microsoft.AspNetCore.Mvc;
using WhatTheWorld.Application.Services.Interfaces;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class NewsController(INewsService newsService) : ControllerBase
    {
        private readonly INewsService _newsService = newsService;

        [HttpGet("cached")]
        public async Task<IActionResult> GetCachedNews([FromQuery] int countryId)
        {
            var news = await _newsService.GetCachedNewsAsync(countryId);
            return Ok(news);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshNews([FromQuery] int countryId)
        {
            var result = await _newsService.RefreshNewsAsync(countryId);

            return Ok(result);
        }
    }
}
