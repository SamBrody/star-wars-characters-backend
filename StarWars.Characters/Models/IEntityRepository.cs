namespace StarWars.Characters.Models;

public interface IEntityRepository<TEntity> where TEntity: class {
    Task<ICollection<TEntity>> GetManyAsync(CancellationToken c);

    Task<TEntity> InsertAsync(TEntity entity, CancellationToken c);
}