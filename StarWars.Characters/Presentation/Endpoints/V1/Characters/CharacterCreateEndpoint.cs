using FastEndpoints;
using MediatR;
using StarWars.Characters.Application.Characters;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Models.Species;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class CharacterCreateEndpoint(ISender sender) : Endpoint<CharacterCreateRequest> {
    public override void Configure() {
        AllowAnonymous();

        Post("/characters");
        Version(1);

        Summary(x => {
            x.Summary = "Create Character";
        });
    }
    
    public override Task HandleAsync(CharacterCreateRequest r, CancellationToken c) {
        var cmd = new RegisterCharacterCommand(
            r.Name,
            r.BirthDay,
            r.PlanetId,
            r.Gender,
            r.SpeciesId,
            r.Height,
            r.HairColor,
            r.EyeColor,
            r.Description,
            r.Movies
        );
        
        return sender.Send(cmd, c);
    }
}

public class CharacterCreateRequest {
    public required string Name { get; init; }
    
    public required CharacterBirthDay BirthDay { get; init; }
    
    public required int PlanetId { get; init; }
    
    public required CharacterGender Gender { get; init; }
    
    public required int SpeciesId { get; init; }
    
    public required int Height { get; init; }
    
    public required string HairColor { get; init; }
    
    public required string EyeColor { get; init; }
    
    public required string Description { get; init; }
    
    public required ICollection<int> Movies { get; init; }
}