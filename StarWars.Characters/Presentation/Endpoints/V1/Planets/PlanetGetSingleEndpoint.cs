using FastEndpoints;
using OneOf;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Planets;

using Result = OneOf<PlanetDto, PlanetGetSingleEndpoint.GetPlanetError>;

public class PlanetGetSingleEndpoint(
    IPlanetRepository planetRepository,
    IMapper mapper
) : Endpoint<PlanetGetSingleEndpoint.PlanetRequest, PlanetDto> {
    public class PlanetRequest {
        [BindFrom("id")]
        public int Id { get; init; }
    }
    
    public enum GetPlanetError {
        PlanetNotFound
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Get("/planets/{id}");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить информацию о планете по её индентификатору";
        });
    }

    public override Task HandleAsync(PlanetRequest r, CancellationToken c) {
        var getCharacterResult = GetCharacterByIdOrDefault(r, c);

        return getCharacterResult.Result.Match(
            dto => SendOkAsync(dto, c),
            e => {
                AddError(e.ToString());
                
                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefault(PlanetRequest r, CancellationToken c) {
        var movie = await planetRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (movie == null) return GetPlanetError.PlanetNotFound;

        var response = mapper.Map<PlanetDto>(movie);
        
        return response;
    }
}