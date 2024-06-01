using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;

namespace StarWars.Characters.Models.Characters;

// The singular form of species is also species
using Species = Species.Species;

public class Character: BaseModel {
    /// <summary>
    /// Имя персонажа
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Дата рождения персонажа
    /// </summary>
    public CharacterBirthDay BirthDay { get; set; }
    
    /// <summary>
    /// Планета происхождения
    /// </summary>
    public Planet HomeWorld { get; set; }
    
    /// <summary>
    /// Пол персонажа
    /// </summary>
    public CharacterGender Gender { get; set; }
    
    /// <summary>
    /// Раса персонажа
    /// </summary>
    public Species Species { get; set; }
    
    /// <summary>
    /// Рост персонажа
    /// </summary>
    public int Height { get; set; }
    
    /// <summary>
    /// Цвет волос персонажа
    /// </summary>
    public string HairColor { get; set; }
    
    /// <summary>
    /// Цвет глаз персонажа
    /// </summary>
    public string EyeColor { get; set; }
    
    /// <summary>
    /// Описание персонажа
    /// </summary>
    public string Description { get; set; }
    
    /// <summary>
    /// Фильмы, в которых персонаж задействован
    /// </summary>
    public ICollection<Movie> Movies { get; set; }
}