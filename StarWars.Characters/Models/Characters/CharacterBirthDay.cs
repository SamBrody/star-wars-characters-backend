namespace StarWars.Characters.Models.Characters;

public class CharacterBirthDay {
    public int Year { get; init; }
    
    public CharacterBirthDayEra Era { get; init; }
}

public enum CharacterBirthDayEra {
    /// <summary>
    /// Before Battle  of Yavin
    /// </summary>
    BBY,
    /// <summary>
    /// After Battle  of Yavin
    /// </summary>
    ABY
}