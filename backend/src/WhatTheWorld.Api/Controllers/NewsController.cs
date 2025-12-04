using Microsoft.AspNetCore.Mvc;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNewsByCountry([FromQuery] int countryId)
        {
            var news = await _newsService.GetCurrentNewsByCountryAsync(countryId);
            return Ok(news);
        }
    }
}
