using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Configuration.Data.Models;

public class CharacterConfiguration : IEntityTypeConfiguration<Character> {
    public void Configure(EntityTypeBuilder<Character> b) {
        b.ToTable("characters");

        b.HasKey(x => x.Id);

        b.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        b.Property(x => x.Name)
            .IsRequired();
        
        b.Property(x => x.OriginalName)
            .IsRequired();

        b.ComplexProperty(x => x.BirthDay, y => {
            y.Property(z => z.Era).IsRequired();
            y.Property(z => z.Year).IsRequired();
        });

        b.HasOne(x => x.HomeWorld)
            .WithMany(y => y.Characters);

        b.Property(x => x.Gender)
            .IsRequired();
        
        b.HasOne(x => x.Species)
            .WithMany(y => y.Characters);
        
        b.Property(x => x.Height)
            .IsRequired();
        
        b.Property(x => x.HairColor)
            .IsRequired();

        b.Property(x => x.EyeColor)
            .IsRequired();

        b.Property(x => x.Description)
            .IsRequired();

        b.HasMany(x => x.Movies)
            .WithMany(y => y.Characters)
            .UsingEntity(z => z.ToTable("characters_movies"));
    }
}