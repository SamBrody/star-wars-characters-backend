using FastEndpoints;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Planets;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Planets;

public class PlanetCreateEndpoint(ISender sender, IMapper mapper) : Endpoint<PlanetCreateEndpoint.CreatePlanetRequest> {
    #region Request

    public class CreatePlanetRequest {
        public required string Name { get; init; }
    }
    
    private class ReqValidator : Validator<CreatePlanetRequest> {
        public ReqValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    #endregion
    
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