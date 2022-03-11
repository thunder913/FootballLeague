using FootballLeague.Builders;
using FootballLeague.Data;
using FootballLeague.Dtos;
using FootballLeague.Models;
using FootballLeague.Models.Enums;
using FootballLeague.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ILeagueService leagueService;
        private readonly ICityService cityService;
        private readonly IMatchService matchService;

        public TeamService(ApplicationDbContext dbContext, ILeagueService leagueService, ICityService cityService, IMatchService matchService)
        {
            this.dbContext = dbContext;
            this.leagueService = leagueService;
            this.cityService = cityService;
            this.matchService = matchService;
        }

        public async Task<int> AddTeamAsync(string teamName, int leagueId, int cityId)
        {
            var league = this.leagueService.GetLeagueById(leagueId);
            if (league == null)
            {
                throw new ArgumentException("There is no league with such name!");
            }

            var city = this.cityService.GetCityById(cityId);
            if (city == null)
            {
                throw new ArgumentException("There is no city with such name!");
            }

            var team = TeamBuilder.Init()
                .SetCity(cityId)
                .SetLeague(leagueId)
                .SetName(teamName)
                .Build();

            try
            {
                await this.dbContext.AddAsync(team);
                await this.dbContext.SaveChangesAsync();
            }catch(Exception)
            {
                throw new Exception("Something went wrong with saving the data, try again!");
            }
            return team.Id;
        }

        public async Task<bool> DeleteTeamByIdAsync(int id)
        {
            var team = this.dbContext.Teams.FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                throw new ArgumentException("There is no team with this id!");
            }
            else if (this.dbContext.Matches.Any(m => m.HomeTeamId == id || m.AwayTeamId == id))
            {
                throw new ArgumentException("There are matches with this team, delete them first!");
            }

            try
            {
                this.dbContext.Teams.Remove(team);
                var removed = await this.dbContext.SaveChangesAsync();
            }catch(Exception)
            {
                throw new Exception("There was an error deleting the team!");
            }

            return true;
        }

        public List<TeamDto> GetAllTeams()
        {
            var teams = this.dbContext.Teams;

            return this.ConvertTeamToDto(teams).ToList();
        }

        public TeamDto GetTeamById(int id)
        {
            var teams = this.dbContext.Teams
                .Where(t => t.Id == id);

            return this.ConvertTeamToDto(teams).FirstOrDefault();
        }

        public List<TeamDto> GetTeamsByLeague(int id)
        {
            var teams =
                this.dbContext
                .Teams
                .Where(t => t.LeagueId == id);

            return this.ConvertTeamToDto(teams).ToList();
        }

        public async Task<int> UpdateTeamInformationAsync(int id, string teamName, int leagueId, int cityId)
        {
            var team = this.dbContext
                .Teams
                .FirstOrDefault(t => t.Id == id);

            if (team == null)
            {
                throw new ArgumentException("There is no team with this id!");
            }else if (!this.cityService.DoesCityExist(cityId))
            {
                throw new ArgumentException("There is no city with this id!");
            }
            else if (!this.leagueService.DoesLeagueExist(leagueId))
            {
                throw new ArgumentException("There is no league with this id!");
            }

            team.Name = teamName;
            team.LeagueId = leagueId;
            team.CityId = cityId;

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("There was an error saving the team into the database!");
            }
            return team.Id;
        }

        private IQueryable<TeamDto> ConvertTeamToDto(IQueryable<Team> teams)
        {
            return teams
                .Include(t => t.City)
                .Include(t => t.League)
                .Include(t => t.HomeMatches)
                .Include(t => t.AwayMatches)
                .Select(t => new TeamDto()
                {
                    CityName = t.City.Name,
                    LeagueName = t.League.Name,
                    Name = t.Name,
                    Points = t.HomeMatches.Count(x => x.Result == MatchResult.HomeWin) * 3 +
                        t.HomeMatches.Count(x => x.Result == MatchResult.Draw) +
                        t.AwayMatches.Count(x => x.Result == MatchResult.AwayWin) * 3 +
                        t.AwayMatches.Count(x => x.Result == MatchResult.Draw),
                    Id = t.Id,
                    CityId = t.CityId,
                    LeagueId = t.LeagueId,
                });
        }
    }
}
