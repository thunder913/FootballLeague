using FootballLeague.Builders;
using FootballLeague.Data;
using FootballLeague.Dtos;
using FootballLeague.Models;
using FootballLeague.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Services
{
    public class MatchService : IMatchService
    {
        private readonly ApplicationDbContext dbContext;

        public MatchService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<int> DeleteMatchesByTeamIdAsync(int teamId)
        {
            var matches = this.dbContext
                    .Matches
                    .Where(m => m.AwayTeamId == teamId || m.HomeTeamId == teamId);

            foreach (var match in matches)
            {
                this.dbContext.Matches.Remove(match);
            }

            try
            {
                return await this.dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("There was an error deleting the data! Try again later!");
            }
        }

        public async Task<int> AddMatchAsync(int homeTeamId, int awayTeamId, int homeGoals, int awayGoals, string date)
        {
            var dateParsed = this.ConvertDateStringToDateTime(date);

            var teamsExist = this.dbContext
                                .Teams
                                .Where(t => t.Id == homeTeamId || t.Id == awayTeamId)
                                .Count();
            if (teamsExist != 2)
            {
                throw new ArgumentException("Teams with such id's don't exist!");
            }

            if (homeGoals < 0 || awayGoals < 0)
            {
                throw new ArgumentException("The goals cannot be a negative value!");
            }

            var match = MatchBuilder
                        .Init()
                        .SetTeams(homeTeamId, awayTeamId)
                        .SetGoals(homeGoals, awayGoals)
                        .SetDate(dateParsed)
                        .Build();

            try
            {
                await this.dbContext.Matches.AddAsync(match);
                await this.dbContext.SaveChangesAsync();
            }catch(Exception)
            {
                throw new Exception("There was an error saving the data! Try again!");
            }

            return match.Id;
        }

        public List<MatchDto> GetAllMatches()
        {
            var matches = this.dbContext
                        .Matches;

            return ConvertMatchToDto(matches).ToList();
        }

        public async Task<int> UpdateMatchAsync(int id, int homeTeamId, int awayTeamId, int homeGoals, int awayGoals, string date)
        {
            if (homeGoals < 0 || awayGoals < 0)
            {
                throw new ArgumentException("The goals cannot be a negative value!");
            }

            var match = this.dbContext
                    .Matches
                    .FirstOrDefault(m => m.Id == id);

            if (match == null)
            {
                throw new ArgumentException("There is no match with such id!");
            }

            match.HomeTeamId = homeTeamId;
            match.AwayTeamId = awayTeamId;
            match.HomeTeamGoals = homeGoals;
            match.AwayTeamGoals = awayGoals;
            match.Date = ConvertDateStringToDateTime(date);

            var teamsCount = this.dbContext
                                .Teams
                                .Where(t => t.Id == homeTeamId || t.Id == awayTeamId)
                                .Count();

            if (teamsCount < 2)
            {
                throw new ArgumentException("There are no teams with such ids!");
            }

            try
            {
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("There was an error updating the entity!");
            }
            return match.Id;
        }

        public async Task DeleteMatchAsync(int id)
        {
            var match = this.dbContext
                            .Matches
                            .FirstOrDefault(m => m.Id == id);
            if (match == null)
            {
                throw new ArgumentException("There is no match with the given id!");
            }

            try
            {
                this.dbContext.Matches.Remove(match);
                await this.dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("There was an error updating the data! Try again!");
            }
        }

        public MatchDto GetMatchById(int id)
        {
            var matches = this.dbContext
                        .Matches
                        .Where(m => m.Id == id);

            var match = this.ConvertMatchToDto(matches).FirstOrDefault();
            if (match == null)
            {
                throw new ArgumentException("There is no match with the given id!");
            }

            return match;
        }

        public List<TeamMatchDto> GetTeamMatches(int teamId)
        {
            var matches = this.dbContext
                    .Matches
                    .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId)
                    .OrderByDescending(m => m.Date)
                    .Select(m => new TeamMatchDto()
                    {
                        Id = m.Id,
                        AwayTeamGoals = m.AwayTeamGoals,
                        AwayTeamId = m.AwayTeamId,
                        Date = m.Date,
                        HomeTeamGoals = m.HomeTeamGoals,
                        HomeTeamId = m.HomeTeamId,
                        HomeTeamName = m.HomeTeam.Name,
                        AwayTeamName = m.AwayTeam.Name
                    }).ToList();

            return matches;
        }

        private DateTime ConvertDateStringToDateTime(string dateString)
        {
            var successfullParse = DateTime.TryParseExact(dateString, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateParsed);
            if (!successfullParse)
            {
                throw new ArgumentException("The date is not in the correct format. The correct format is dd/MM/YYYY hh:mm");
            }

            return dateParsed;
        }

        private IQueryable<MatchDto> ConvertMatchToDto(IQueryable<Match> matches)
        {
            return matches
                .Select(m => new MatchDto()
                {
                    Id = m.Id,
                    AwayTeamGoals = m.AwayTeamGoals,
                    AwayTeamId = m.AwayTeamId,
                    Date = m.Date,
                    HomeTeamGoals = m.HomeTeamGoals,
                    HomeTeamId = m.HomeTeamId
                });
        }
    }
}
