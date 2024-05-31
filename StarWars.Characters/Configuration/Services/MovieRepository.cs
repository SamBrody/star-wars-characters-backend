using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Models.Movies;

namespace StarWars.Characters.Configuration.Services;

public sealed class MovieRepository(StarWarsCharactersDbContext context) : IMovieRepository {
    public async Task<ICollection<Movie>> GetManyAsync(CancellationToken c) {
        var movies = await context.Movies.ToListAsync(c);

        return movies;
    }
    
    public async Task<Movie?> GetByIdOrDefaultAsync(int id, CancellationToken c) {
        var movie = await context.Movies.FirstOrDefaultAsync(x => x.Id == id, c);

        return movie;
    }
    
    public async Task<int> InsertAsync(Movie movie, CancellationToken c) {
        var result = await context.Movies.AddAsync(movie, c);

        return result.Entity.Id;
    }

    public async Task<ICollection<Movie>?> GetRangeByIdsOrDefaultAsync(ICollection<int> ids, CancellationToken c) {
        var movies = await context.Movies.Where(x => ids.Contains(x.Id)).ToListAsync(c);

        return movies;
    }
}