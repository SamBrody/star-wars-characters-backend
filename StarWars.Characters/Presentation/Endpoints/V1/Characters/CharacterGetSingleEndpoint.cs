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

    public override Task HandleAsync(CharacterRequest r, CancellationToken c) {
        var getCharacterResult = GetCharacterByIdOrDefaultAsync(r, c);

        return getCharacterResult.Result.Match(
            dto => SendOkAsync(dto, c),
            e => {
                AddError(e.ToString());
                
                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefaultAsync(CharacterRequest r, CancellationToken c) {
        var character = await characterRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (character == null) return GetCharacterError.CharacterNotFound;

        var response = mapper.Map<CharacterDto>(character);
        
        return response;
    }
}