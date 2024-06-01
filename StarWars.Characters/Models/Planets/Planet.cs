using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Models.Planets;

public class Planet : BaseModel {
    public string Name { get; set; }
    
    public ICollection<Character> Characters { get; set; }
}