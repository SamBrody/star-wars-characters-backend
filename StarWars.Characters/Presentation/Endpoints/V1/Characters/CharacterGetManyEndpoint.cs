using FastEndpoints;
using StarWars.Characters.Models.Characters;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class CharacterGetManyEndpoint(ICharacterRepository characterRepository) : EndpointWithoutRequest {
    public override void Configure() {
        AllowAnonymous();
        
        Get("/characters");
        Version(1);
        
        Summary(x => {
            x.Summary = "Get Characters Many";
        });
    }

    public override async Task HandleAsync(CancellationToken c) {
        var characters = await characterRepository.GetManyAsync(c);

        await SendOkAsync(characters, cancellation: c);
    }
}