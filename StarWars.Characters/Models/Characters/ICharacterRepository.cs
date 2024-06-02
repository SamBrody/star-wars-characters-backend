using StarWars.Characters.Configuration.Utils.Filtering;

namespace StarWars.Characters.Models.Characters;

using FilteringParams = CharacterFilteringParams;

public interface ICharacterRepository : IEntityRepository<Character> {
    Task<ICollection<Character>> GetManyFilteredAsync(FilteringParams filteringParams, CancellationToken c);
    
    Character Upsert(Character character);
    
    void Remove(Character character);
}