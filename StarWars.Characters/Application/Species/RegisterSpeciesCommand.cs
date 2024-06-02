using MediatR;
using OneOf;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Application.Species;

using Command = RegisterSpeciesCommand;
using Result = OneOf<int, CreateSpeciesError>;
using Species = Models.Species.Species;

public record RegisterSpeciesCommand(string Name) : IRequest<Result>, ITransactional;

internal class RegisterSpeciesCommandHandler(ISpeciesRepository speciesRepository) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) => await CreateSpeciesAsync(cmd, c);

    private async Task<Result> CreateSpeciesAsync(Command cmd, CancellationToken c) {
        var newMovie = new Species { Name = cmd.Name, };
        
        return await speciesRepository.InsertAsync(newMovie, c);
    }
}

public enum CreateSpeciesError { }