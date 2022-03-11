namespace FootballLeague.Dtos
{
    public class TeamRankDto
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public int Points { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
    }
}
