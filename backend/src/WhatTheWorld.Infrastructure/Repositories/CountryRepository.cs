using Microsoft.EntityFrameworkCore;
using WhatTheWorld.Domain;
using WhatTheWorld.Infrastructure.Data;

namespace WhatTheWorld.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;

        public CountryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Country?> GetByCodeAsync(string code)
        {
            return await _context.Countries
                .FirstOrDefaultAsync(c => c.Code == code);
        }
    }
}
