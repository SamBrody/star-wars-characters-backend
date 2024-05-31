namespace StarWars.Characters.Models;

public interface IEntityRepository<TEntity> where TEntity: class {
    Task<ICollection<TEntity>> GetManyAsync(CancellationToken c);
    
    Task<TEntity?> GetByIdOrDefaultAsync(int id, CancellationToken c);

    Task<int> InsertAsync(TEntity entity, CancellationToken c);
}