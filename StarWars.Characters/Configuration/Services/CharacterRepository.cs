using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Configuration.Services;

public sealed class CharacterRepository(StarWarsCharactersDbContext context) : ICharacterRepository {
    public async Task<ICollection<Character>> GetManyAsync(CancellationToken c) {
        var characters = await context.Characters
            .Include(x => x.Species)
            .Include(x => x.HomeWorld)
            .Include(x => x.Movies)
            .ToListAsync(c);

        return characters;
    }

    public async Task<Character?> GetByIdOrDefaultAsync(int id, CancellationToken c) {
        var character = await context.Characters
            .Include(x => x.Species)
            .Include(x => x.HomeWorld)
            .Include(x => x.Movies)
            .FirstOrDefaultAsync(x => x.Id == id, c);

        return character;
    }
    
    public async Task<int> InsertAsync(Character character, CancellationToken c) {
        var result = await context.Characters.AddAsync(character, c);

        return result.Entity.Id;
    }

    public Character Upsert(Character character) => context.Characters.Update(character).Entity;

    public void Remove(Character character) => context.Characters.Remove(character);
}