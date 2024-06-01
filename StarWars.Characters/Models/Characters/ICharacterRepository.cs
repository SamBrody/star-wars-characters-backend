namespace StarWars.Characters.Models.Characters;

public interface ICharacterRepository : IEntityRepository<Character> {
    Character Upsert(Character character);
    
    void Remove(Character character);
}