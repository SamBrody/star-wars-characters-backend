using FastEndpoints;
using OneOf;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Movies;

using Result = OneOf<MovieDto, MovieGetSingleEndpoint.GetMovieError>;

public class MovieGetSingleEndpoint(
    IMovieRepository movieRepository,
    IMapper mapper
) : Endpoint<MovieGetSingleEndpoint.MovieRequest, MovieDto> {
    public class MovieRequest {
        [BindFrom("id")]
        public int Id { get; init; }
    }
    
    public enum GetMovieError {
        MovieNotFound
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Get("/movies/{id}");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить информацию о фильме по его индентификатору";
        });
    }

    public override async Task HandleAsync(MovieRequest r, CancellationToken c) {
        var getCharacterResult = await GetCharacterByIdOrDefault(r, c);

        await getCharacterResult.Match(
            async dto => await SendOkAsync(dto, c),
            async e => {
                AddError(e.ToString());
                
                await SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefault(MovieRequest r, CancellationToken c) {
        var movie = await movieRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (movie == null) return GetMovieError.MovieNotFound;

        var response = mapper.Map<MovieDto>(movie);
        
        return response;
    }
}