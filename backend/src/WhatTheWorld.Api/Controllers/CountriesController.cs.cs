using Microsoft.AspNetCore.Mvc;
using WhatTheWorld.Application.Services.Interfaces;

namespace WhatTheWorld.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class CountriesController(ICountryService countryService) : ControllerBase
    {
        private readonly ICountryService _countryService = countryService;

        [HttpGet]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryService.GetAllCountriesAsync();
            return Ok(countries);
        }
    }
}
