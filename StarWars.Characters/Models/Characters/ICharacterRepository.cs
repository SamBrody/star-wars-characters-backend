namespace StarWars.Characters.Models.Characters;

public interface ICharacterRepository : IEntityRepository<Character> {
    Task RemoveAsync(Character character, CancellationToken c);
}