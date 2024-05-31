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

    public override async Task HandleAsync(SpeciesRequest r, CancellationToken c) {
        var getCharacterResult = await GetCharacterByIdOrDefault(r, c);

        await getCharacterResult.Match(
            async dto => await SendOkAsync(dto, c),
            async e => {
                AddError(e.ToString());
                
                await SendErrorsAsync(cancellation: c);
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