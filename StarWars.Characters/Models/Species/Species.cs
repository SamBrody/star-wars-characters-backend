using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Models.Species;

public class Species : BaseModel {
    public string Name { get; init; }
    
    public ICollection<Character> Characters { get; init; }
}