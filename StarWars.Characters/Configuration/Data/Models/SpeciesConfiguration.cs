using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Configuration.Data.Models;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species> {
    public void Configure(EntityTypeBuilder<Species> b) {
        b.ToTable("species");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        b.Property(x => x.Name)
            .IsRequired();
    }
}