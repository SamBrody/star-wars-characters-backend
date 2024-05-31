using FastEndpoints;
using StarWars.Characters.Models.Movies;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Movies;

using Response = ICollection<MovieGetManyResponse>;

public class MovieGetManyEndpoint(IMovieRepository movieRepository, IMapper mapper) : EndpointWithoutRequest<Response> {
    public override void Configure() {
        AllowAnonymous();
        
        Get("/movies");
        Version(1);
        
        Summary(x => {
            x.Summary = "Get Many Movies";
        });
    }
    
    public override async Task HandleAsync(CancellationToken c) {
        var movies = await movieRepository.GetManyAsync(c);
        var response = mapper.Map<Response>(movies);
        
        await SendOkAsync(response, cancellation: c);
    }
}

public class MovieGetManyResponse {
    public required int Id { get; init; }
    
    public required string Name { get; init; }
}