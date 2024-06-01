using FastEndpoints;
using MediatR;
using StarWars.Characters.Application.Characters;
using StarWars.Characters.Models.Characters;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class CharacterCreateEndpoint(ISender sender, IMapper mapper) : Endpoint<CreateCharacterRequest> {
    public override void Configure() {
        AllowAnonymous();

        Post("/characters");
        Version(1);

        Summary(x => {
            x.Summary = "Регистрация нового персонажа";
        });
    }
    
    public override Task HandleAsync(CreateCharacterRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterCharacterCommand>(r);
        
        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}

public class CreateCharacterRequest {
    public required string Name { get; init; }
    
    public required CharacterBirthDay BirthDay { get; init; }
    
    public required int PlanetId { get; init; }
    
    public required CharacterGender Gender { get; init; }
    
    public required int SpeciesId { get; init; }
    
    public required int Height { get; init; }
    
    public required string HairColor { get; init; }
    
    public required string EyeColor { get; init; }
    
    public required string Description { get; init; }
    
    public required ICollection<int> MovieIds { get; init; }
}