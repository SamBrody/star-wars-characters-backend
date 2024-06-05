using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Configuration.Utils.Filtering;

public record CharacterFilteringParams(
    int? YearLowerBound = null,
    int? YearUpperBound = null,
    int? HomeWorldId = null,
    CharacterGender? Gender = null,
    ICollection<int>? MoviesIds = null
);