using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Models.Planets;

namespace StarWars.Characters.Configuration.Services;

public class PlanetRepository(StarWarsCharactersDbContext context) : IPlanetRepository {
    public async Task<ICollection<Planet>> GetManyAsync(CancellationToken c) {
        var planets = await context.Planets.ToListAsync(c);

        return planets;
    }

    public async Task<Planet?> GetByIdOrDefaultAsync(int id, CancellationToken c) {
        var planet = await context.Planets.FirstOrDefaultAsync(x => x.Id == id, c);

        return planet;
    }

    public async Task<int> InsertAsync(Planet planet, CancellationToken c) {
        var result = await context.Planets.AddAsync(planet, c);

        return result.Entity.Id;
    }
}