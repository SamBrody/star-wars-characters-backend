namespace StarWars.Characters.Models.Users;

public interface IUserRepository : IEntityRepository<User> {
    Task<User?> GetUserByLoginOrDefaultAsync(string login, CancellationToken c);
}