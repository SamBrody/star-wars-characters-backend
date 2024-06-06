using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Presentation.Dtos;

public class CharacterDto {
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public string OriginalName { get; init; }
    
    public CharacterBirthDay BirthDay { get; init; }
    
    public PlanetDto HomeWorld { get; init; }
    
    public CharacterGender Gender { get; init; }
    
    public SpeciesDto Species { get; init; }
    
    public UserDto User { get; init; }
    
    public int Height { get; init; }
    
    public string HairColor { get; init; }
    
    public string EyeColor { get; init; }
    
    public string Description { get; init; }
    
    public ICollection<MovieDto> Movies { get; init; }
}