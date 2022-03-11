using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Dtos
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LeagueName { get; set; }
        public int LeagueId { get; set; }
        public string CityName { get; set; }
        public int CityId { get; set; }
        public int Points { get; set; }
    }
}
