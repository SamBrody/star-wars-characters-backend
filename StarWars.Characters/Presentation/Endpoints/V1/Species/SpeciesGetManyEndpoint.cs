using FastEndpoints;
using StarWars.Characters.Models.Species;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Species;

using Response = ICollection<SpeciesDto>;

public class SpeciesGetManyEndpoint(
    ISpeciesRepository speciesRepository,
    IMapper mapper
) : EndpointWithoutRequest<Response> {
    public override void Configure() {
        AllowAnonymous();
        
        Get("/species");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить список рас";
        });
    }
    
    public override async Task HandleAsync(CancellationToken c) {
        var movies = await speciesRepository.GetManyAsync(c);
        var response = mapper.Map<Response>(movies);
        
        await SendOkAsync(response, cancellation: c);
    }
}