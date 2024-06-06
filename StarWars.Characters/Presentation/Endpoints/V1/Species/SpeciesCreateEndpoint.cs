using FastEndpoints;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Species;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Species;

public class SpeciesCreateEndpoint(ISender sender, IMapper mapper) : Endpoint<SpeciesCreateEndpoint.CreateSpeciesRequest> {
    #region Request

    public class CreateSpeciesRequest {
        public required string Name { get; init; }
    }
    
    private class ReqValidator : Validator<CreateSpeciesRequest> {
        public ReqValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    #endregion
    
    public override void Configure() {
        Post("/species");
        Version(1);
        Validator<ReqValidator>();

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