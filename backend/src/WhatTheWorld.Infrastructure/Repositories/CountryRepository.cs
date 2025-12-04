using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;
using WhatTheWorld.Infrastructure.Mappings;
using WhatTheWorld.Infrastructure.Repositories.Interfaces;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public sealed class CountryRepository(AppDbContext context) : ICountryRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<List<CountryDto>> GetAllCountriesAsync()
        {
            var result = await _context.Countries
                .Select(c => c.ToDto())
                .ToListAsync();
            return result!;
        }
        public async Task<string> GetCountryNameByIdAsync(int countryId)
        {
            var result = await _context.Countries
                .Where(c => c.Id == countryId)
                .Select(c => c.Name)
                .FirstOrDefaultAsync();
            return result ?? string.Empty;
        }

        public async Task<CountryDto?> GetCountryByIdAsync(int Id)
        {
            var result = await _context.Countries.FirstOrDefaultAsync(c => c.Id == Id);
            return result?.ToDto();
        }

        public async Task<CountryEntity?> GetCountryWithDetailsAsync(string code)
        {
            return await _context.Countries
                .Include(c => c.News)
                .Include(c => c.Weather)
                .FirstOrDefaultAsync(c => c.Code == code);
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
