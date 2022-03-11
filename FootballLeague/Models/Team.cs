using FootballLeague.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class Team
    {
        public int Id{ get; set; }
        [Required]
        public string Name { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Match> HomeMatches { get; set; } = new HashSet<Match>();
        public virtual ICollection<Match> AwayMatches { get; set; } = new HashSet<Match>();
        public virtual League League { get; set; }
        public int LeagueId { get; set; }
    }
}
