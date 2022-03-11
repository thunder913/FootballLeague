using FootballLeague.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballLeague.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<Team> Teams { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Match> Matches { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Match>()
                .HasOne(x => x.HomeTeam)
                .WithMany(x => x.HomeMatches)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(x => x.AwayTeam)
                .WithMany(x => x.AwayMatches)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}