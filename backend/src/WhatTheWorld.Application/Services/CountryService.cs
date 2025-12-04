using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services
{
    public sealed class CountryService(ICountryRepository repository) : ICountryService
    {
        private readonly ICountryRepository _repository = repository;

        public async Task<List<CountryDto>> GetAllCountriesAsync()
        {
            var countries = await _repository.GetAllCountriesAsync();
            return countries;
        }
    }
}
