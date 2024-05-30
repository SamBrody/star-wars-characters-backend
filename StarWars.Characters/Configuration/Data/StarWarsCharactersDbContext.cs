using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data.Models;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Configuration.Data;

public class StarWarsCharactersDbContext : DbContext {
    public StarWarsCharactersDbContext(DbContextOptions<StarWarsCharactersDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CharacterConfiguration).Assembly);
        
        {
            modelBuilder.Entity<Species>().HasData(
                new {Id = 1, Name = "Человек", Characters = new List<Character>()},
                new {Id = 2, Name = "Раса йоды", Characters = new List<Character>()}
            );
        
            modelBuilder.Entity<Planet>().HasData(
                new {Id = 1, Name = "Татуин ", Characters = new List<Character>()},
                new {Id = 2, Name = "Альдераан", Characters = new List<Character>()}
            );
        
            modelBuilder.Entity<Movie>().HasData(
                new {Id = 1, Name = "Звездные войны: Скрытая угроза ", Characters = new List<Character>()},
                new {Id = 2, Name = "Звездные войны: Атака клонов", Characters = new List<Character>()}
            );
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseSnakeCaseNamingConvention();
    }

    public DbSet<Character> Characters { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Planet> Planets { get; set; }
    public DbSet<Species> Species { get; set; }
}