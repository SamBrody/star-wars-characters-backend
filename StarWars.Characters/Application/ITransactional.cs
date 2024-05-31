using System.Data;

namespace StarWars.Characters.Application;

/// <summary>
/// Маркерный интрефейс для открытия транзакции в рамках пайплайна
/// </summary>
public interface ITransactional {
    public IsolationLevel IsolationLevel => IsolationLevel.ReadUncommitted;
}