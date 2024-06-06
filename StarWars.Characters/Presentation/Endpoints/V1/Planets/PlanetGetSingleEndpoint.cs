using FastEndpoints;
using FluentValidation;
using OneOf;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Planets;

using Result = OneOf<PlanetDto, PlanetGetSingleEndpoint.GetPlanetError>;

public class PlanetGetSingleEndpoint(
    IPlanetRepository planetRepository,
    IMapper mapper
) : Endpoint<PlanetGetSingleEndpoint.Request, PlanetDto> {
    #region Request

    public class Request {
        [BindFrom("id")]
        public int Id { get; init; }
    }

    private class ReqValidator : Validator<Request> {
        public ReqValidator() {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Неверный идентификатор");
        }
    }

    #endregion
    
    public enum GetPlanetError {
        PlanetNotFound
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Get("/planets/{id}");
        Version(1);
        Validator<ReqValidator>();
        
        Summary(x => {
            x.Summary = "Получить информацию о планете по её индентификатору";
        });
    }

    public override Task HandleAsync(Request r, CancellationToken c) {
        var getCharacterResult = GetCharacterByIdOrDefault(r, c);

        return getCharacterResult.Result.Match(
            dto => SendOkAsync(dto, c),
            e => {
                AddError(e.ToString());
                
                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefault(Request r, CancellationToken c) {
        var movie = await planetRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (movie == null) return GetPlanetError.PlanetNotFound;

        var response = mapper.Map<PlanetDto>(movie);
        
        return response;
    }
}