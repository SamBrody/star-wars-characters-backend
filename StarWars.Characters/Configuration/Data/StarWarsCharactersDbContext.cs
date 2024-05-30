﻿using Microsoft.EntityFrameworkCore;
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