using FastEndpoints;
using MediatR;
using StarWars.Characters.Application.Planets;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Planets;

public class PlanetCreateEndpoint(ISender sender, IMapper mapper) : Endpoint<CreatePlanetRequest> {
    public override void Configure() {
        AllowAnonymous();

        Post("/planets");
        Version(1);

        Summary(x => {
            x.Summary = "Регистрация новой планеты";
        });
    }
    
    public override async Task HandleAsync(CreatePlanetRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterPlanetCommand>(r);
        
        await sender.Send(cmd, c).Result.Match(
            async _ => await SendOkAsync(c),
            async e => {
                AddError(e.ToString());

                await SendErrorsAsync(cancellation: c);
            }
        );
    }
}

public class CreatePlanetRequest {
    public required string Name { get; init; }
}