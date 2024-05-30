using MediatR;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Application.Characters;

using Command = RegisterCharacterCommand;

public record RegisterCharacterCommand(
    string Name, CharacterBirthDay BirthDay, int PlanetId, CharacterGender Gender, int SpeciesId, int Height,
    string HairColor, string EyeColor, string Description, ICollection<int> Movies
) : IRequest<Character>;

public sealed class RegisterCharacterCommandHandler : IRequestHandler<Command, Character> {
    private readonly ICharacterRepository characterRepository;
    
    public RegisterCharacterCommandHandler(ICharacterRepository characterRepository) {
        this.characterRepository = characterRepository;
    }

    public async Task<Character> Handle(Command cmd, CancellationToken c) {
        var newCharacter = new Character {
            // Name = cmd.Name,
            // BirthDay = cmd.BirthDay,
            // HomeWorld = cmd.PlanetId,
            // Gender = cmd.Gender,
            // Species = cmd.SpeciesId,
            // Height = cmd.Height,
            // HairColor = cmd.HairColor,
            // EyeColor = cmd.EyeColor,
            // Description = cmd.Description,
            // Movies = cmd.Movies,
        };

        return await characterRepository.InsertAsync(newCharacter, c);
    }
}