using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballLeague.Dtos
{
    public class TeamMatchDto : MatchDto
    {
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
    }
}
