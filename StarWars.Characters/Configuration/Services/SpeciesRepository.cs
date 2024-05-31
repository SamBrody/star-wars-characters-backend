using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Configuration.Services;

public sealed class SpeciesRepository(StarWarsCharactersDbContext context) : ISpeciesRepository {
    public async Task<ICollection<Species>> GetManyAsync(CancellationToken c) {
        var species = await context.Species.ToListAsync(c);

        return species;
    }

    public async Task<Species?> GetByIdOrDefaultAsync(int id, CancellationToken c) {
        var species = await context.Species.FirstOrDefaultAsync(x => x.Id == id, c);

        return species;
    }

    public async Task<int> InsertAsync(Species species, CancellationToken c) {
        var result = await context.Species.AddAsync(species, c);

        return result.Entity.Id;
    }
}