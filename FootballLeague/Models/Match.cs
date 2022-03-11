using FootballLeague.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballLeague.Models
{
    public class Match
    {
        public Match()
        {

        }
        public int Id { get; set; }
        public int HomeTeamId { get; set; }
        public virtual Team HomeTeam { get; set; }
        public int AwayTeamId{ get; set; }
        public virtual Team AwayTeam { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public MatchResult Result { get; set; }
        public DateTime Date { get; set; }
    }
}
