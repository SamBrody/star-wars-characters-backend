using Bogus;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Configuration.Data;

public class DatabaseSeeder {
    public List<Movie> Movies { get; } = [];
    public List<Planet> Planets { get; } = [];
    public List<Species> Species { get; } = [];

    private static readonly List<string> moviesNames = [
        "Звездные войны: Эпизод 4 — Новая надежда",
        "Звездные войны: Эпизод 5 — Империя наносит ответный удар",
        "Звездные войны: Эпизод 6 — Возвращение джедая",
        "Звездные войны: Эпизод 1 — Скрытая угроза",
        "Звездные войны: Эпизод 2 — Атака клонов",
        "Звездные войны: Эпизод 3 — Месть ситхов",
        "Звездные войны: Пробуждение силы",
        "Изгой-один: Звездные войны. Истории",
        "Звездные войны: Последние джедаи",
        "Хан Соло: Звездные войны. Истории",
        "Звездные войны: Скайуокер. Восход"
    ];

    private static readonly List<string> planetsNames = [
        "Корусант",
        "Хосниан-Прайм",
        "Альдераан",
        "Набу",
        "Татуин",
        "Коррибан",
        "Датомир",
        "Камино",
        "Мандалор",
        "Кашиик",
    ];
    
    private static readonly List<string> speceisNames = [
        "Раса Йода",
        "Вуки",
        "Человек",
        "Кали",
        "Хатт",
        "Тви’лек",
        "Дурос",
        "Юужань-вонги",
        "Талз",
        "Куаррен",
        "Эвоки",
    ];

    public DatabaseSeeder() {
        Movies = GenerateMovies(10);
        Planets = GeneratePlanets(10);
        Species = GenerateSpecies(10);
    }

    private static List<Movie> GenerateMovies(int amount) {
        var movieId = 1;
        var nameId = 0;
        var faker = new Faker<Movie>()
            .RuleFor(x => x.Id, _ => movieId++)
            .RuleFor(x => x.Name, _ => moviesNames[nameId++]);

        var movies = Enumerable.Range(1, amount)
            .Select(i => SeedRow(faker, i))
            .ToList();

        return movies;
    }
    
    private static List<Planet> GeneratePlanets(int amount) {
        var planetId = 1;
        var nameId = 0;
        var faker = new Faker<Planet>()
            .RuleFor(x => x.Id, _ => planetId++)
            .RuleFor(x => x.Name, _ => planetsNames[nameId++]);

        var planets = Enumerable.Range(1, amount)
            .Select(i => SeedRow(faker, i))
            .ToList();

        return planets;
    }
    
    private static List<Species> GenerateSpecies(int amount) {
        var speciesId = 1;
        var nameId = 0;
        var faker = new Faker<Species>()
            .RuleFor(x => x.Id, _ => speciesId++)
            .RuleFor(x => x.Name, _ => speceisNames[nameId++]);

        var species = Enumerable.Range(1, amount)
            .Select(i => SeedRow(faker, i))
            .ToList();

        return species;
    }
    
    private static T SeedRow<T>(Faker<T> faker, int rowId) where T : class => faker.UseSeed(rowId).Generate();
}