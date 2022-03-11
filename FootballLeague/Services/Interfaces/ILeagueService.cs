using FootballLeague.Dtos;
using FootballLeague.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface ILeagueService
    {
        League GetLeagueByName(string name);
        League GetLeagueById(int id);
        bool DoesLeagueExist(int id);
        List<LeagueDto> GetLeagues();
        LeagueDto GetLeagueDtoById(int id);
        Task<int> AddLeagueAsync(string name, int countryId);

        Task<League> UpdateLeagueAsync(int id, string name, int countryId);

        Task DeleteCountryAsync(int id);
        LeagueRankingDto GetLeagueRanking(int id);
    }
}
