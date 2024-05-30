using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarWars.Characters.Models.Movies;

namespace StarWars.Characters.Configuration.Data.Models;

public class MovieConfiguration : IEntityTypeConfiguration<Movie> {
    public void Configure(EntityTypeBuilder<Movie> b) {
        b.ToTable("movies");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        b.Property(x => x.Name)
            .IsRequired();
    }
}