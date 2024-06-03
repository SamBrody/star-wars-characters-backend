using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Configuration.Utils.Filtering;

public record CharacterFilteringParams(
    int? BornYearFrom = null,
    int? BornYearTo = null,
    int? HomeWorldId = null,
    CharacterGender? Gender = null,
    ICollection<int>? MoviesIds = null
);