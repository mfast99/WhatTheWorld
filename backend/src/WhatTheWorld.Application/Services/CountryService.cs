using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Repositories;

namespace WhatTheWorld.Application.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _repository;

        public CountryService(ICountryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Country?> GetByCodeAsync(string code)
        {
            return await _repository.GetByCodeAsync(code);
        }
    }
}
