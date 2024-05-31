using FastEndpoints;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

using Response = ICollection<CharacterDto>;

public class CharacterGetManyEndpoint(
    ICharacterRepository characterRepository,
    IMapper mapper
) : EndpointWithoutRequest<Response> {
    public override void Configure() {
        AllowAnonymous();
        
        Get("/characters");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить всех персонажей";
        });
    }

    public override async Task HandleAsync(CancellationToken c) {
        var characters = await characterRepository.GetManyAsync(c);
        var response = mapper.Map<Response>(characters);
        
        await SendOkAsync(response, cancellation: c);
    }
}