using Microsoft.AspNetCore.Mvc;
using WhatTheWorld.Application.Services.Interfaces;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentWeather([FromQuery] int countryId)
        {
            var weather = await _weatherService.GetCurrentWeatherByCountryIdAsync(countryId);
            if (weather == null) return NotFound("No weather data found");
            return Ok(weather);
        }
    }
}
