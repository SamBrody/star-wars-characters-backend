using FastEndpoints;
using MediatR;
using StarWars.Characters.Application.Characters;
using StarWars.Characters.Models.Characters;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

using Request = UpdateCharacterRequest;

public class CharacterUpdateEndpoint(ISender sender, IMapper mapper) : Endpoint<Request> {
    public override void Configure() {
        AllowAnonymous();
        
        Put("characters/{id}");

        Summary(s => {
            s.Summary = "Обновление параметров Персонажа";
        });
    }

    public override Task HandleAsync(Request r, CancellationToken c) {
        var cmd = mapper.Map<UpdateCharacterCommand>(r);

        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}

public class UpdateCharacterRequest {
    [BindFrom("id")]
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public CharacterBirthDay BirthDay { get; init; }
    
    public int PlanetId { get; init; }
    
    public CharacterGender Gender { get; init; }
    
    public int SpeciesId { get; init; }
    
    public int Height { get; init; }
    
    public string HairColor { get; init; }
    
    public string EyeColor { get; init; }
    
    public string Description { get; init; }
    
    public ICollection<int> MovieIds { get; init; }
}