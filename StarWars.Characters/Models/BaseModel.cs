namespace StarWars.Characters.Models;
/// <summary>
/// Базовый класс для моделей, содержащий Id
/// </summary>
public abstract class BaseModel {
    public long Id { get; init; }
}