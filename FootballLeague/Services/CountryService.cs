using FootballLeague.Data;
using FootballLeague.Dtos;
using FootballLeague.Models;
using FootballLeague.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext dbContext;

        public CountryService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<CountryDto> GetCountries()
        {
            return this.dbContext
                    .Countries
                    .Select(c => new CountryDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).ToList();
        }

        public CountryDto GetCountryById(int id)
        {
            return this.dbContext
                    .Countries
                    .Where(c => c.Id == id)
                    .Select(c => new CountryDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                    }).FirstOrDefault();
        }

        public async Task<int> AddCountryAsync(string name)
        {
            var country = new Country()
            {
                Name = name,
            };

            await this.dbContext.AddAsync(country);
            await this.dbContext.SaveChangesAsync();
            return country.Id;
        }

        public async Task<Country> UpdateCountryAsync(int id, string name)
        {
            var country = this.dbContext
                            .Countries
                            .FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                throw new ArgumentException("The is no country with the provided id!");
            }

            country.Name = name;

            await this.dbContext.SaveChangesAsync();
            return country;
        }

        public async Task DeleteCountryAsync(int id)
        {
            var country = this.dbContext.Countries
                .Include(c => c.Leagues)
                .Include(c => c.Cities)
                .FirstOrDefault(c => c.Id == id);
            if (country == null)
            {
                throw new ArgumentException("There is no country with the provided id!");
            }
            else if (country.Leagues.Any())
            {
                throw new ArgumentException("There are leagues, who play in this country. Make sure you move them to another one!");
            }
            else if (country.Cities.Any())
            {
                throw new ArgumentException("There are cities, located in this country. Make sure you move them!");
            }

            this.dbContext.Countries.Remove(country);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
