using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Models.Movies;

public class Movie {
    public string Name { get; init; }
    
    /// <summary>
    /// Персонажи, зайдестованные в фильме
    /// </summary>
    public ICollection<Character> Characters { get; init; }
}