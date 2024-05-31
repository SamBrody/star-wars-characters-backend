namespace StarWars.Characters.Models.Movies;

public interface IMovieRepository : IEntityRepository<Movie> {
    Task<ICollection<Movie>?> GetRangeByIdsOrDefaultAsync(ICollection<int> ids, CancellationToken c);
}