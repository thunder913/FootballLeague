using System.Collections.Generic;

namespace FootballLeague.Dtos
{
    public class LeagueRankingDto
    {
        public int Id{ get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public List<TeamRankDto> Teams { get; set; } = new List<TeamRankDto>();
    }
}
