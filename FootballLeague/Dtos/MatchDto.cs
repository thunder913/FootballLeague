using System;

namespace FootballLeague.Dtos
{
    public class MatchDto
    {
        public int Id{ get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public DateTime Date { get; set; }
        public string DateString => Date.ToString("dd/MM/yyyy hh:mm");
    }
}
