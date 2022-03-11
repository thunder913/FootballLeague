using FootballLeague.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Builders
{
    public class TeamBuilder
    {
        private Team _team = new Team();

        public static TeamBuilder Init()
        {
            return new TeamBuilder();
        }
        public TeamBuilder SetCity(int cityId)
        {
            this._team.CityId = cityId;
            return this;
        }

        public TeamBuilder SetLeague(int leagueId)
        {
            this._team.LeagueId = leagueId;
            return this;
        }

        public TeamBuilder SetName(string name)
        {
            this._team.Name = name;
            return this;
        }

        public Team Build() => this._team;
    }
}
