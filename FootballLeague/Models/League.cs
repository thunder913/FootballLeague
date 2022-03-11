using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class League
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Team> Teams { get; set; } = new HashSet<Team>();
        public virtual Country Country { get; set; }
        public int CountryId { get; set; }
    }
}
