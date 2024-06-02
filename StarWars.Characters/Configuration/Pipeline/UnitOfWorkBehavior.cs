using MediatR;
using StarWars.Characters.Application;
using StarWars.Characters.Configuration.Data;
using Microsoft.EntityFrameworkCore;

namespace StarWars.Characters.Configuration.Pipeline;

/// <summary>
/// Поведение на транзакционные запросы для фиксации изменений в БД
/// </summary>
internal class UnitOfWorkBehavior<TRequest, TResponse> :
    IPipelineBehavior<TRequest, TResponse> where TRequest : ITransactional {
    private StarWarsCharactersDbContext dbContext;
    
    public UnitOfWorkBehavior(StarWarsCharactersDbContext dbContext) {
        this.dbContext = dbContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken c) {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(request.IsolationLevel, c);

        try {
            var response = await next();

            await dbContext.SaveChangesAsync(c);

            await transaction.CommitAsync(c);

            return response;
        } catch {
            await transaction.RollbackAsync(c);
        
            throw;
        }
    }
}