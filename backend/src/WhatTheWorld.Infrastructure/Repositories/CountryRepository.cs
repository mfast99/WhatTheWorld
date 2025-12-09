using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Mappings;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public sealed class CountryRepository(AppDbContext context, ILogger<CountryRepository> logger) : ICountryRepository
    {
        private readonly AppDbContext _context = context;
        private readonly ILogger<CountryRepository> _logger = logger;

        public async Task<List<CountryListDto>> GetAllCountriesAsync()
        {
            var result = await _context.Countries
                .Select(c => c.ToListDto())
                .ToListAsync();
            return result;
        }

        public async Task<CountryDto> GetCountryByIdAsync(int Id)
        {
            try
            {
                var result = await _context.Countries.FirstOrDefaultAsync(c => c.Id == Id);
                return result!.ToDto();
            }
            catch (Exception)
            {
                _logger.LogError("Country doesnt exist.");
                throw;
            }
        }

        public async Task<string> GetCountryNameByIdAsync(int id)
        {
            return _context.Countries.Where(c => c.Id == id)
                .Select(c => c.Name)
                .FirstOrDefault() ?? string.Empty;
        }

        public async Task<int> CreateCountryAsync(CountryEntity country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();
            return country.Id;
        }

        public async Task<bool> UpdateCountryAsync(CountryEntity country)
        {
            var existingCountry = await _context.Countries.FirstOrDefaultAsync(c => c.Id == country.Id);
            if (existingCountry == null) return false;
            existingCountry.Name = country.Name;
            existingCountry.Population = country.Population;
            existingCountry.AreaKm2 = country.AreaKm2;
            _context.Countries.Update(existingCountry);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteCountryAsync(string code)
        {
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Code == code);
            if (country == null) return false;
            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
