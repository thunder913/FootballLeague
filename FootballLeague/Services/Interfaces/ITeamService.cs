using FootballLeague.Dtos;
using FootballLeague.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface ITeamService
    {
        Task<int> AddTeamAsync(string teamName, int leagueId, int cityId);
        Task<int> UpdateTeamInformationAsync(int id, string teamName, int leagueId, int cityId);
        Task<bool> DeleteTeamByIdAsync(int id);
        TeamDto GetTeamById(int id);
        List<TeamDto> GetAllTeams();
        List<TeamDto> GetTeamsByLeague(int id);
    }
}
