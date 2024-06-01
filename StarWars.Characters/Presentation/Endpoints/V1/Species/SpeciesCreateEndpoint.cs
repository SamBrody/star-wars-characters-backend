using FastEndpoints;
using MediatR;
using StarWars.Characters.Application.Species;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Species;

public class SpeciesCreateEndpoint(ISender sender, IMapper mapper) : Endpoint<CreateSpeciesRequest> {
    public override void Configure() {
        AllowAnonymous();

        Post("/species");
        Version(1);

        Summary(x => {
            x.Summary = "Регистрация новой расы";
        });
    }
    
    public override async Task HandleAsync(CreateSpeciesRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterSpeciesCommand>(r);
        
        await sender.Send(cmd, c).Result.Match(
            async _ => await SendOkAsync(c),
            async e => {
                AddError(e.ToString());

                await SendErrorsAsync(cancellation: c);
            }
        );
    }
}

public class CreateSpeciesRequest {
    public required string Name { get; init; }
}