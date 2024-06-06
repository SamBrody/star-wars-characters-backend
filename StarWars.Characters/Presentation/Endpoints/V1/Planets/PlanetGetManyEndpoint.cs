using FastEndpoints;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Planets;

using Response = ICollection<PlanetDto>;

public class PlanetGetManyEndpoint(
    IPlanetRepository planetRepository,
    IMapper mapper
) : EndpointWithoutRequest<Response> {
    public override void Configure() {
        Get("/planets");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить список планет";
        });
    }
    
    public override async Task HandleAsync(CancellationToken c) {
        var movies = await planetRepository.GetManyAsync(c);
        var response = mapper.Map<Response>(movies);
        
        await SendOkAsync(response, cancellation: c);
    }
}