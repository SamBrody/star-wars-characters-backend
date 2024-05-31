using MediatR;
using OneOf;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Application.Characters;

using Command = RegisterCharacterCommand;
using Result = OneOf<int, CreateCharacterError>;

public record RegisterCharacterCommand (
    string Name, CharacterBirthDay BirthDay, int PlanetId, CharacterGender Gender, int SpeciesId, int Height,
    string HairColor, string EyeColor, string Description, ICollection<int> MovieIds
) : IRequest<Result>, ITransactional;

public sealed class RegisterCharacterCommandHandler(
    ICharacterRepository characterRepository,
    IMovieRepository movieRepository,
    ISpeciesRepository speciesRepository,
    IPlanetRepository planetRepository
) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) {
        var createCharacterResult = await CreateCharacter(cmd, c);
        return createCharacterResult;
    }
    
    private async Task<Result> CreateCharacter(Command cmd, CancellationToken c) {
        var movies = await movieRepository.GetRangeByIdsOrDefaultAsync(cmd.MovieIds, c);
        var planet = await planetRepository.GetByIdOrDefaultAsync(cmd.PlanetId, c);
        var species = await speciesRepository.GetByIdOrDefaultAsync(cmd.SpeciesId, c);

        if (movies == null) return CreateCharacterError.MoviesNotFound;
        if (planet == null) return CreateCharacterError.HoweWorldNotFound;
        if (species == null) return CreateCharacterError.SpeciesIsNotFound;

        var newCharacter = new Character {
            Name = cmd.Name,
            BirthDay = cmd.BirthDay,
            HomeWorld = planet,
            Gender = cmd.Gender,
            Species = species,
            Height = cmd.Height,
            HairColor = cmd.HairColor,
            EyeColor = cmd.EyeColor,
            Description = cmd.Description,
            Movies = movies,
        };
        
        return await characterRepository.InsertAsync(newCharacter, c);
    }
}

public enum CreateCharacterError {
    HoweWorldNotFound,
    SpeciesIsNotFound,
    MoviesNotFound,
}