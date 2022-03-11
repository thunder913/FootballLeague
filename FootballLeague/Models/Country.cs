using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Models
{
    public class Country
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<City> Cities { get; set; } = new HashSet<City>();
        public virtual ICollection<League> Leagues { get; set; } = new HashSet<League>();
    }
}
