using FastEndpoints;
using MediatR;
using StarWars.Characters.Application.Movies;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Movies;

public class MovieCreateEndpoint(ISender sender, IMapper mapper) : Endpoint<CreateMovieRequest> {
    public override void Configure() {
        AllowAnonymous();

        Post("/movies");
        Version(1);

        Summary(x => {
            x.Summary = "Регистрация нового фильма";
        });
    }
    
    public override async Task HandleAsync(CreateMovieRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterMovieCommand>(r);
        
        await sender.Send(cmd, c).Result.Match(
            async _ => await SendOkAsync(c),
            async e => {
                AddError(e.ToString());

                await SendErrorsAsync(cancellation: c);
            }
        );
    }
}

public class CreateMovieRequest {
    public required string Name { get; init; }
}