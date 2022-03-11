
using FootballLeague.Models;
using FootballLeague.Models.Enums;
using System;

namespace FootballLeague.Builders
{
    public class MatchBuilder
    {
        private Match _match = new Match();

        public static MatchBuilder Init()
        {
            return new MatchBuilder();
        }
        public MatchBuilder SetTeams(int homeTeamId, int awayTeamId)
        {
            this._match.HomeTeamId = homeTeamId;
            this._match.AwayTeamId = awayTeamId;
            return this;
        }

        public MatchBuilder SetGoals(int homeTeamGoals, int awayTeamGoals)
        {
            this._match.HomeTeamGoals = homeTeamGoals;
            this._match.AwayTeamGoals = awayTeamGoals;
            if (homeTeamGoals > awayTeamGoals)
            {
                this._match.Result = MatchResult.HomeWin;
            }
            else if (homeTeamGoals == awayTeamGoals)
            {
                this._match.Result = MatchResult.Draw;
            }
            else
            {
                this._match.Result = MatchResult.AwayWin;
            }
            return this;
        }

        public MatchBuilder SetDate(DateTime date)
        {
            this._match.Date = date;
            return this;
        }

        public Match Build() => this._match;
    }
}
