using MediatR;
using OneOf;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Application.Characters;

using Command = UpdateCharacterCommand;
using Result = OneOf<Character, UpdateCharacterErrors>;

public record UpdateCharacterCommand(
    int               Id,
    string            Name,
    CharacterBirthDay BirthDay,
    int               PlanetId, 
    CharacterGender   Gender, 
    int               SpeciesId,
    int               Height,
    string            HairColor,
    string            EyeColor,
    string            Description,
    ICollection<int>  MovieIds
) : IRequest<Result>, ITransactional;

internal class UpdateCharacterCommandHandler(
    ICharacterRepository characterRepository,
    IMovieRepository movieRepository,
    ISpeciesRepository speciesRepository,
    IPlanetRepository planetRepository
) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) => await UpdateCharacterAsync(cmd, c);

    private async Task<Result> UpdateCharacterAsync(Command cmd, CancellationToken c) {
        var character = await characterRepository.GetByIdOrDefaultAsync(cmd.Id, c);
        if (character == null) return UpdateCharacterErrors.CharacterNotFound;

        var planet = await planetRepository.GetByIdOrDefaultAsync(cmd.PlanetId, c);
        if (planet == null) return UpdateCharacterErrors.HoweWorldNotFound;
        
        var species = await speciesRepository.GetByIdOrDefaultAsync(cmd.SpeciesId, c);
        if (species == null) return UpdateCharacterErrors.SpeciesIsNotFound;
        
        var movies = await movieRepository.GetRangeByIdsOrDefaultAsync(cmd.MovieIds, c);
        if (movies == null) return UpdateCharacterErrors.MoviesNotFound;

        character.Name        = cmd.Name;
        character.BirthDay    = cmd.BirthDay;
        character.HomeWorld   = planet;
        character.Gender      = cmd.Gender;
        character.Species     = species;
        character.Height      = cmd.Height;
        character.HairColor   = cmd.HairColor;
        character.EyeColor    = cmd.EyeColor;
        character.Description = cmd.Description;
        character.Movies      = movies;
        
        return characterRepository.Upsert(character);
    }
}

public enum UpdateCharacterErrors {
    CharacterNotFound,
    HoweWorldNotFound,
    SpeciesIsNotFound,
    MoviesNotFound,
};