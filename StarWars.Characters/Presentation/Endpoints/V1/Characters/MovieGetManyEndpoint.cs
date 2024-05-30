using FastEndpoints;
using StarWars.Characters.Models.Movies;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class MovieGetManyEndpoint(IMovieRepository movieRepository) : EndpointWithoutRequest {
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

        await SendOkAsync(movies, cancellation: c);
    }
}