using FastEndpoints;
using FluentValidation;
using OneOf;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

using Result = OneOf<CharacterDto, CharacterGetSingleEndpoint.GetCharacterError>;

public class CharacterGetSingleEndpoint(
    ICharacterRepository characterRepository,
    IMapper mapper
) : Endpoint<CharacterGetSingleEndpoint.Request, CharacterDto> {
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
    
    public enum GetCharacterError {
        CharacterNotFound
    }
    
    public override void Configure() {AllowAnonymous();
        AllowAnonymous();
        
        Get("/characters/{id}");
        Version(1);
        Validator<ReqValidator>();
        
        Summary(x => {
            x.Summary = "Получить детальную информацию об персонажи по его индентификатору";
        });
    }

    public override Task HandleAsync(Request r, CancellationToken c) {
        var getCharacterResult = GetCharacterByIdOrDefaultAsync(r, c);

        return getCharacterResult.Result.Match(
            dto => SendOkAsync(dto, c),
            e => {
                AddError(e.ToString());
                
                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefaultAsync(Request r, CancellationToken c) {
        var character = await characterRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (character == null) return GetCharacterError.CharacterNotFound;

        var response = mapper.Map<CharacterDto>(character);
        
        return response;
    }
}