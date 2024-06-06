using MediatR;
using OneOf;
using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Application.Characters;

using Command = RemoveCharacterCommand;
using Result = OneOf<bool, RemoveCharacterError>;

public record RemoveCharacterCommand(int Id, int UserId) : IRequest<Result>, ITransactional;

internal class RemoveCharacterCommandHandler(ICharacterRepository characterRepository) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) => await RemoveCharacterAsync(cmd, c);

    private async Task<Result> RemoveCharacterAsync(Command cmd, CancellationToken c) {
        var character = await characterRepository.GetByIdOrDefaultAsync(cmd.Id, c);
        
        if (character == null) return RemoveCharacterError.CharacterNotFound;
        if (character.CreatedBy.Id != cmd.UserId) return RemoveCharacterError.OnlyAuthorCanRemoveCharacter;

        characterRepository.Remove(character);
        
        return true;
    }
}

public enum RemoveCharacterError {
    CharacterNotFound,
    OnlyAuthorCanRemoveCharacter,
}