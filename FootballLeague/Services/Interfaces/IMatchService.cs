using FootballLeague.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootballLeague.Services.Interfaces
{
    public interface IMatchService
    {
        Task<int> DeleteMatchesByTeamIdAsync(int teamId);
        Task<int> AddMatchAsync(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals, string date);
        List<MatchDto> GetAllMatches();
        Task<int> UpdateMatchAsync(int id, int homeTeamId, int awayTeamId, int homeGoals, int awayGoals, string date);
        Task DeleteMatchAsync(int id);
        MatchDto GetMatchById(int id);
        List<TeamMatchDto> GetTeamMatches(int teamId);
    }
}
