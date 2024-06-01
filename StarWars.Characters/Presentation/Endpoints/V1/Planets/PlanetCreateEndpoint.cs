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
    
    public override Task HandleAsync(CreatePlanetRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterPlanetCommand>(r);
        
        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}

public class CreatePlanetRequest {
    public required string Name { get; init; }
}