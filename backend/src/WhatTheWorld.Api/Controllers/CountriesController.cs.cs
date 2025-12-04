using Microsoft.AspNetCore.Mvc;
using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetCountry(string code)
        {
            CountryDto? country = await _countryService.GetByCodeAsync(code);
            if (country == null) return NotFound();
            return Ok(country);
        }
    }
}
