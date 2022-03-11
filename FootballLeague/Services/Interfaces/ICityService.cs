using FootballLeague.Dtos;
using FootballLeague.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface ICityService
    {
        City GetCityByName(string name);
        City GetCityById(int id);
        Task<int> AddCityAsync(string name, int countryId);
        bool DoesCityExist(int id);
        List<CityDto> GetCities();
        CityDto GetCityDtoById(int id);
        Task<int> UpdateCityAsync(int id, string name, int countryId);
        Task DeleteCityAsync(int id);
    }
}
