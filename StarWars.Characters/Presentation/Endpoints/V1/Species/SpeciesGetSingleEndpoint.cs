using FastEndpoints;
using OneOf;
using StarWars.Characters.Models.Species;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Species;

using Result = OneOf<SpeciesDto, SpeciesGetSingleEndpoint.GetSpeciesError>;

public class SpeciesGetSingleEndpoint(
    ISpeciesRepository speciesRepository,
    IMapper mapper
) : Endpoint<SpeciesGetSingleEndpoint.SpeciesRequest, SpeciesDto> {
    public class SpeciesRequest {
        [BindFrom("id")]
        public int Id { get; init; }
    }
    
    public enum GetSpeciesError {
        SpeciesNotFound
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Get("/species/{id}");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить информацию о планете по её индентификатору";
        });
    }

    public override Task HandleAsync(SpeciesRequest r, CancellationToken c) {
        var getCharacterResult = GetCharacterByIdOrDefault(r, c);

        return getCharacterResult.Result.Match(
            dto => SendOkAsync(dto, c),
            e => {
                AddError(e.ToString());
                
                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefault(SpeciesRequest r, CancellationToken c) {
        var movie = await speciesRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (movie == null) return GetSpeciesError.SpeciesNotFound;

        var response = mapper.Map<SpeciesDto>(movie);
        
        return response;
    }
}