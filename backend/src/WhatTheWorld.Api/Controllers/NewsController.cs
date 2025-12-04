using Microsoft.AspNetCore.Mvc;
using WhatTheWorld.Application.Services.Interfaces;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class NewsController(INewsService newsService) : ControllerBase
    {
        private readonly INewsService _newsService = newsService;

        [HttpGet]
        public async Task<IActionResult> GetNewsByCountry([FromQuery] int countryId)
        {
            var news = await _newsService.GetNewsByCountryIdAsync(countryId);
            return Ok(news);
        }
    }
}
