using FastEndpoints;
using StarWars.Characters.Models.Characters;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

using Response = ICollection<CharacterGetManyResponse>;

public class CharacterGetManyEndpoint(
    ICharacterRepository characterRepository,
    IMapper mapper
) : EndpointWithoutRequest<Response> {
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
        var response = mapper.Map<Response>(characters);
        
        await SendOkAsync(response, cancellation: c);
    }
}

public class CharacterGetManyResponse {
    public int Id { get; init; }
    
    public string Name { get; init; }
    
    public CharacterBirthDay BirthDay { get; init; }
    
    public PlanetDto HomeWorld { get; init; }
    
    public CharacterGender Gender { get; init; }
    
    public SpeciesDto Species { get; init; }
    
    public int Height { get; init; }
    
    public string HairColor { get; init; }
    
    public string EyeColor { get; init; }
    
    public string Description { get; init; }
    
    public ICollection<MovieDto> Movies { get; init; }
}

public class PlanetDto {
    public int Id { get; init; }
    
    public string Name { get; init; }
}

public class SpeciesDto {
    public int Id { get; init; }
    
    public string Name { get; init; }
}

public class MovieDto {
    public int Id { get; init; }
    
    public string Name { get; init; }
}