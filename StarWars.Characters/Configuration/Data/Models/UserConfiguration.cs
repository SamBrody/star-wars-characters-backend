using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StarWars.Characters.Models.Users;

namespace StarWars.Characters.Configuration.Data.Models;

public class UserConfiguration : IEntityTypeConfiguration<User> {
    public void Configure(EntityTypeBuilder<User> b) {
        b.ToTable("users");

        b.HasKey(x => x.Id);
        
        b.Property(x => x.Id)
            .ValueGeneratedOnAdd();

        b.Property(x => x.Login)
            .IsRequired();
        
        b.Property(x => x.Password)
            .IsRequired();
    }
}