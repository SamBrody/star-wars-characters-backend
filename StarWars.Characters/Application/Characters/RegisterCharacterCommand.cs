using MediatR;
using OneOf;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;
using StarWars.Characters.Models.Users;

namespace StarWars.Characters.Application.Characters;

using Command = RegisterCharacterCommand;
using Result = OneOf<int, CreateCharacterError>;

public record RegisterCharacterCommand (
    string            Name,
    string            OriginalName,
    CharacterBirthDay BirthDay,
    int               PlanetId, 
    CharacterGender   Gender, 
    int               SpeciesId,
    int               CreatedById,
    int               Height,
    string            HairColor,
    string            EyeColor,
    string            Description,
    ICollection<int>  MovieIds
) : IRequest<Result>, ITransactional;

internal class RegisterCharacterCommandHandler(
    ICharacterRepository characterRepository,
    IMovieRepository movieRepository,
    ISpeciesRepository speciesRepository,
    IPlanetRepository planetRepository,
    IUserRepository userRepository
) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) => await CreateCharacterAsync(cmd, c);

    private async Task<Result> CreateCharacterAsync(Command cmd, CancellationToken c) {
        var movies = await movieRepository.GetRangeByIdsOrDefaultAsync(cmd.MovieIds, c);
        var planet = await planetRepository.GetByIdOrDefaultAsync(cmd.PlanetId, c);
        var species = await speciesRepository.GetByIdOrDefaultAsync(cmd.SpeciesId, c);
        var user = await userRepository.GetByIdOrDefaultAsync(cmd.CreatedById, c);

        if (movies == null) return CreateCharacterError.MoviesNotFound;
        if (planet == null) return CreateCharacterError.HoweWorldNotFound;
        if (species == null) return CreateCharacterError.SpeciesIsNotFound;
        if (user == null) return CreateCharacterError.UserNotFound; 

        var newCharacter = new Character {
            Name         = cmd.Name,
            OriginalName = cmd.OriginalName,
            BirthDay     = cmd.BirthDay,
            HomeWorld    = planet,
            Gender       = cmd.Gender,
            Species      = species,
            CreatedBy    = user, 
            Height       = cmd.Height,
            HairColor    = cmd.HairColor,
            EyeColor     = cmd.EyeColor,
            Description  = cmd.Description,
            Movies       = movies,
        };
        
        return await characterRepository.InsertAsync(newCharacter, c);
    }
}

public enum CreateCharacterError {
    HoweWorldNotFound,
    SpeciesIsNotFound,
    MoviesNotFound,
    UserNotFound,
}