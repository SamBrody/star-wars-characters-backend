using FastEndpoints;
using OneOf;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

using Result = OneOf<CharacterDto, CharacterGetSingleEndpoint.GetCharacterError>;

public class CharacterGetSingleEndpoint(
    ICharacterRepository characterRepository,
    IMapper mapper
) : Endpoint<CharacterGetSingleEndpoint.CharacterRequest, CharacterDto> {
    public class CharacterRequest {
        [BindFrom("id")]
        public int Id { get; init; }
    }
    
    public enum GetCharacterError {
        CharacterNotFound
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Get("/characters/{id}");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить детальную информацию об персонажи по его индентификатору";
        });
    }

    public override async Task HandleAsync(CharacterRequest r, CancellationToken c) {
        var getCharacterResult = await GetCharacterByIdOrDefault(r, c);

        await getCharacterResult.Match(
            async dto => await SendOkAsync(dto, c),
            async e => {
                AddError(e.ToString());
                
                await SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefault(CharacterRequest r, CancellationToken c) {
        var character = await characterRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (character == null) return GetCharacterError.CharacterNotFound;

        var response = mapper.Map<CharacterDto>(character);
        
        return response;
    }
}