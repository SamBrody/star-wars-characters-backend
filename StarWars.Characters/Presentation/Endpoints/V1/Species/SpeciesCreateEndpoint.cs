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
    
    public override Task HandleAsync(CreateSpeciesRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterSpeciesCommand>(r);
        
        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}

public class CreateSpeciesRequest {
    public required string Name { get; init; }
}