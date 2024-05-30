using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarWars.Characters.Models.Planets;

namespace StarWars.Characters.Configuration.Data.Models;

public class PlanetConfiguration : IEntityTypeConfiguration<Planet> {
    public void Configure(EntityTypeBuilder<Planet> b) {
        b.ToTable("planets");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        b.Property(x => x.Name)
            .IsRequired();
    }
}