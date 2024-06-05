using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Configuration.Utils.Filtering;
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

    public async Task<ICollection<Character>> GetManyFilteredAsync(CharacterFilteringParams fp, CancellationToken c) {
        var characters = context.Characters
            .Include(x => x.Species)
            .Include(x => x.HomeWorld)
            .Include(x => x.Movies)
            .AsNoTracking();

        if (fp.HomeWorldId != null) characters = characters.Where(x => x.HomeWorld.Id == fp.HomeWorldId);
        if (fp.Gender != null) characters = characters.Where(x => x.Gender == fp.Gender);
        if (fp.MoviesIds != null) characters = characters.Where(x => x.Movies.Select(y => y.Id).Intersect(fp.MoviesIds).Any());
        if (fp.YearLowerBound != null) characters = characters.Where(x => x.BirthDay.Year >= fp.YearLowerBound);
        if (fp.YearUpperBound != null) characters = characters.Where(x => x.BirthDay.Year <= fp.YearUpperBound);
        
        return await characters.ToListAsync(c);
    }

    public Character Upsert(Character character) => context.Characters.Update(character).Entity;

    public void Remove(Character character) => context.Characters.Remove(character);
}