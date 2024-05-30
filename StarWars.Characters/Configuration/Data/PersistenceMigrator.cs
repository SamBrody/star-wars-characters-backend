using Microsoft.EntityFrameworkCore;

namespace StarWars.Characters.Configuration.Data;

public class PersistenceMigrator<TDbContext>(IServiceProvider sp) : IHostedService where TDbContext : DbContext {
    public async Task StartAsync(CancellationToken c) {
        // Создаем новый scope, т.к. HostService регистрируется как Singleton в Composition Root
        using var serviceScope = sp.CreateScope();

        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<PersistenceMigrator<TDbContext>>>();

        logger.LogDebug("Running database migrations");
        
        var dbContext = serviceScope.ServiceProvider.GetRequiredService<TDbContext>();
        
        var pendingMigrationsResult = await dbContext.Database.GetPendingMigrationsAsync(c);
        
        var pendingMigrations = pendingMigrationsResult.ToList();

        if (pendingMigrations.Count > 0) {
            logger.LogDebug("No pending migrations to apply");
            
            return;
        }
        
        logger.LogDebug("There are {Migrations} migrations to apply", pendingMigrations.Count);
        
        await dbContext.Database.EnsureCreatedAsync(c);

        await dbContext.Database.MigrateAsync(c);
    }

    public async Task StopAsync(CancellationToken cancellationToken) { }
}