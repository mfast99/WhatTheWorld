using WhatTheWorld.Application.Services.Interfaces;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Application.Services
{
    public sealed class CountryService(ICountryRepository repository) : ICountryService
    {
        private readonly ICountryRepository _repository = repository;

        public async Task<List<CountryListDto>> GetAllCountriesAsync()
        {
            var countries = await _repository.GetAllCountriesAsync();
            return countries;
        }

        public async Task<CountryDto> GetCountryByIdAsync(int id)
        {
            var result = await _repository.GetCountryByIdAsync(id);
            return result;
        }
    }
}
