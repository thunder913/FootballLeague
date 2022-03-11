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
    public class LeagueService : ILeagueService
    {
        private readonly ApplicationDbContext dbContext;

        public LeagueService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public League GetLeagueById(int id)
        {
            return dbContext.Leagues.FirstOrDefault(l => l.Id == id);
        }

        public League GetLeagueByName(string name)
        {
            return dbContext.Leagues.FirstOrDefault(l => l.Name == name);
        }

        public bool DoesLeagueExist(int id)
        {
            return this.dbContext.Leagues.FirstOrDefault(l => l.Id == id) != null;
        }

        public List<LeagueDto> GetLeagues()
        {
            var leagues = this.dbContext
                            .Leagues;

            return ConvertLeagueToDto(leagues).ToList();
        }

        public LeagueDto GetLeagueDtoById(int id)
        {
            var leagues = this.dbContext
                            .Leagues
                            .Where(l => l.Id == id);

            var league = ConvertLeagueToDto(leagues).FirstOrDefault();
            return league;
        }

        public async Task<int> AddLeagueAsync(string name, int countryId)
        {
            if (!DoesCountryExist(countryId))
            {
                throw new ArgumentException("There is no country with the given id!");
            }
            var league = new League()
            {
                Name = name,
                CountryId = countryId,
            };

            await this.dbContext.AddAsync(league);
            await this.dbContext.SaveChangesAsync();
            return league.Id;
        }

        public async Task<League> UpdateLeagueAsync(int id, string name, int countryId)
        {
            var league = this.dbContext
                            .Leagues
                            .FirstOrDefault(l => l.Id == id);

            if (league == null)
            {
                throw new ArgumentException("There is no league with this id!");
            }
            else if (!DoesCountryExist(countryId))
            {
                throw new ArgumentException("The is no country with the provided id!");
            }

            league.Name = name;
            league.CountryId = countryId;

            await this.dbContext.SaveChangesAsync();
            return league;
        }

        public async Task DeleteCountryAsync(int id)
        {
            var league = this.dbContext.Leagues
                .Include(c => c.Teams)
                .FirstOrDefault(c => c.Id == id);
            if (league == null)
            {
                throw new ArgumentException("There is no country with the provided id!");
            }
            else if (league.Teams.Any())
            {
                throw new ArgumentException("There are teams, who play in this league. Make sure you move them to another one!");
            }

            this.dbContext.Leagues.Remove(league);
            await this.dbContext.SaveChangesAsync();
        }

        public LeagueRankingDto GetLeagueRanking(int id)
        {
            return this.dbContext
                            .Leagues
                            .Include(l => l.Teams)
                            .Include(l => l.Country)
                            .Select(l => new LeagueRankingDto()
                            {
                                Id = l.Id,
                                Name = l.Name,
                                CountryName = l.Country.Name,
                                Teams = l.Teams.Select(t => new TeamRankDto()
                                {
                                    TeamName = t.Name,
                                    Id = t.Id,
                                    Points = t.HomeMatches.Count(x => x.Result == MatchResult.HomeWin) * 3 +
                                    t.HomeMatches.Count(x => x.Result == MatchResult.Draw) +
                                    t.AwayMatches.Count(x => x.Result == MatchResult.AwayWin) * 3 +
                                    t.AwayMatches.Count(x => x.Result == MatchResult.Draw),
                                    GoalsScored = t.HomeMatches.Sum(x => x.HomeTeamGoals) + t.AwayMatches.Sum(x => x.AwayTeamGoals),
                                    GoalsConceded = t.HomeMatches.Sum(x => x.AwayTeamGoals) + t.AwayMatches.Sum(x => x.HomeTeamGoals),
                                })
                                .OrderByDescending(t => t.Points)
                                .ThenByDescending(t => t.GoalsScored - t.GoalsConceded)
                                .ThenByDescending(t => t.GoalsScored)
                                .ToList()
                            })
                            .FirstOrDefault(l => l.Id == id);

        }

        private IQueryable<LeagueDto> ConvertLeagueToDto(IQueryable<League> leagues)
        {
            return leagues
                    .Include(l => l.Country)
                    .Select(l => new LeagueDto()
                    {
                        CountryId = l.CountryId,
                        CountryName = l.Country.Name,
                        Id = l.Id,
                        Name = l.Name,
                    });
        }

        private bool DoesCountryExist(int id)
        {
            return dbContext.Countries.Any(c => c.Id == id);
        }
    }
}
