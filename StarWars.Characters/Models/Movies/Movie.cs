﻿using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Models.Movies;

public class Movie : BaseModel {
    public string Name { get; set; }
    
    /// <summary>
    /// Персонажи, зайдестованные в фильме
    /// </summary>
    public ICollection<Character> Characters { get; set; }
}