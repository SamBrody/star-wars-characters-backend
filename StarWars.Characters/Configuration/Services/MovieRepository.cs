using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Models.Movies;

namespace StarWars.Characters.Configuration.Services;

public sealed class MovieRepository(StarWarsCharactersDbContext context) : IMovieRepository {
    public async Task<ICollection<Movie>> GetManyAsync(CancellationToken c) {
        var movies = await context.Movies.ToListAsync(c);

        return movies;
    }
    
    public async Task<Movie> InsertAsync(Movie movie, CancellationToken c) {
        var result = await context.Movies.AddAsync(movie, c);

        return result.Entity;
    }
}