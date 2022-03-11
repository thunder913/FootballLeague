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
    public class CityService : ICityService
    {
        private readonly ApplicationDbContext dbContext;

        public CityService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<int> AddCityAsync(string name, int countryId)
        {
            if (!DoesCountryExist(countryId))
            {
                throw new ArgumentException("There is no country with this id!");
            }

            var city = new City()
            {
                Name = name,
                CountryId = countryId,
            };

            try
            {
                await this.dbContext.AddAsync(city);
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("There was an error with saving the city, try again!");
            }
            return city.Id;
        }

        public City GetCityById(int id)
        {
            return this.dbContext.Cities.FirstOrDefault(c => c.Id == id);
        }

        public CityDto GetCityDtoById(int id)
        {
            return this.dbContext
                        .Cities
                        .Select(c => new CityDto()
                        {
                            CountryId = c.CountryId,
                            Id = c.Id,
                            Name = c.Name
                        })
                        .FirstOrDefault(c => c.Id == id);
        }

        public City GetCityByName(string name)
        {
            return this.dbContext.Cities.FirstOrDefault(c => c.Name == name);
        }

        public bool DoesCityExist(int id)
        {
            return this.dbContext.Cities.FirstOrDefault(c => c.Id == id) != null;
        }

        public List<CityDto> GetCities()
        {
            return this.dbContext
                    .Cities
                    .Select(c => new CityDto()
                    {
                        CountryId = c.CountryId,
                        Id = c.Id,
                        Name = c.Name
                    }).ToList();
        }

        public async Task<int> UpdateCityAsync(int id, string name, int countryId)
        {
            var city = this.dbContext
                            .Cities
                            .FirstOrDefault(c => c.Id == id);

            if (city == null)
            {
                throw new ArgumentException("There is no city with this id!");
            }

            if (!DoesCountryExist(countryId))
            {
                throw new ArgumentException("There is no country with this id!");
            }

            city.Name = name;
            city.CountryId = countryId;

            await this.dbContext.SaveChangesAsync();

            return city.Id;
        }

        public async Task DeleteCityAsync(int id)
        {
            var city = this.dbContext.Cities
                        .Include(c => c.Teams)
                        .FirstOrDefault(c => c.Id == id);
            if (city == null)
            {
                throw new ArgumentException("There is no city with the provided id!");
            }
            else if (city.Teams.Any())
            {
                throw new ArgumentException("There are teams, who play in this city. Make sure you move them to another one!");
            }

            this.dbContext.Cities.Remove(city);
            await this.dbContext.SaveChangesAsync();
        }

        private bool DoesCountryExist(int id)
        {
            return this.dbContext
                            .Countries
                            .Where(c => c.Id == id)
                            .Any();
        }
    }
}
