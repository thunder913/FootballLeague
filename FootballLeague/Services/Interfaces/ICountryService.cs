using FootballLeague.Dtos;
using FootballLeague.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface ICountryService
    {
        List<CountryDto> GetCountries();

        CountryDto GetCountryById(int id);

        Task<int> AddCountryAsync(string name);
        Task<Country> UpdateCountryAsync(int id, string name);

        Task DeleteCountryAsync(int id);
    }
}
