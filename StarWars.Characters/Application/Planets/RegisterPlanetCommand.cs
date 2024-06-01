using MediatR;
using OneOf;
using StarWars.Characters.Models.Planets;

namespace StarWars.Characters.Application.Planets;

using Command = RegisterPlanetCommand;
using Result = OneOf<int, CreatePlanetError>;

public record RegisterPlanetCommand(string Name) : IRequest<Result>, ITransactional;

public sealed class RegisterPlanetCommandHandler(IPlanetRepository planetRepository) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) => await CreatePlanetAsync(cmd, c);

    private async Task<Result> CreatePlanetAsync(Command cmd, CancellationToken c) {
        var newPlanet = new Planet { Name = cmd.Name };

        return await planetRepository.InsertAsync(newPlanet, c);
    }
}

public enum CreatePlanetError { }