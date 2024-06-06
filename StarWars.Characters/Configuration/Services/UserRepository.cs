using Microsoft.EntityFrameworkCore;
using StarWars.Characters.Configuration.Data;
using StarWars.Characters.Models.Users;

namespace StarWars.Characters.Configuration.Services;

public class UserRepository(StarWarsCharactersDbContext context) : IUserRepository {
    public async Task<ICollection<User>> GetManyAsync(CancellationToken c) {
        var users = await context.Users.ToListAsync(c);

        return users;
    }

    public async Task<User?> GetByIdOrDefaultAsync(int id, CancellationToken c) {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id, c);

        return user;
    }

    public async Task<int> InsertAsync(User user, CancellationToken c) {
        var result = await context.Users.AddAsync(user, c);

        return result.Entity.Id;
    }

    public async Task<User?> GetUserByLoginOrDefaultAsync(string login, CancellationToken c) {
        var user = await context.Users.FirstOrDefaultAsync(x => x.Login == login, c);

        return user;
    }
}