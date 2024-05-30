using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Races;

namespace StarWars.Characters.Models.Characters;

public class Character: BaseModel {
    /// <summary>
    /// Имя персонажа
    /// </summary>
    public string Name { get; init; }
    
    /// <summary>
    /// Дата рождения персонажа
    /// </summary>
    public CharacterBirthDay BirthDay { get; init; }
    
    /// <summary>
    /// Планета происхождения
    /// </summary>
    public Planet HomeWorld { get; init; }
    
    /// <summary>
    /// Пол персонажа
    /// </summary>
    public CharacterGender Gender { get; init; }
    
    /// <summary>
    /// Раса персонажа
    /// </summary>
    public Species Species { get; init; }
    
    /// <summary>
    /// Рост персонажа
    /// </summary>
    public int Height { get; init; }
    
    /// <summary>
    /// Цвет волос персонажа
    /// </summary>
    public string HairColor { get; init; }
    
    /// <summary>
    /// Цвет глаз персонажа
    /// </summary>
    public string EyeColor { get; init; }
    
    /// <summary>
    /// Описание персонажа
    /// </summary>
    public string Description { get; init; }
    
    /// <summary>
    /// Фильмы, в которых персонаж задействован
    /// </summary>
    public ICollection<Movie> Movies { get; init; }
}