using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Configuration.Services;

public sealed class CharacterRepository(StarWarsCharactersDbContext context) : ICharacterRepository {
    public async Task<ICollection<Character>> GetManyAsync(CancellationToken c) {
        var characters = await context.Characters.ToListAsync(c);

        return characters;
    }

    public async Task<Character> InsertAsync(Character character, CancellationToken c) {
        var result = await context.Characters.AddAsync(character, c);

        return result.Entity;
    }
}