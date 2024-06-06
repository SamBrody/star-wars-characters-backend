using FastEndpoints;
using FluentValidation;
using OneOf;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Presentation.Dtos;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Movies;

using Result = OneOf<MovieDto, MovieGetSingleEndpoint.GetMovieError>;

public class MovieGetSingleEndpoint(
    IMovieRepository movieRepository,
    IMapper mapper
) : Endpoint<MovieGetSingleEndpoint.Request, MovieDto> {
    #region Request

    public class Request {
        [BindFrom("id")]
        public int Id { get; init; }
    }

    private class ReqValidator : Validator<Request> {
        public ReqValidator() {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Неверный идентификатор");
        }
    }

    #endregion
    
    public enum GetMovieError {
        MovieNotFound
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Get("/movies/{id}");
        Version(1);
        Validator<ReqValidator>();
        
        Summary(x => {
            x.Summary = "Получить информацию о фильме по его индентификатору";
        });
    }

    public override Task HandleAsync(Request r, CancellationToken c) {
        var getCharacterResult = GetCharacterByIdOrDefaultAsync(r, c);

        return getCharacterResult.Result.Match(
            dto => SendOkAsync(dto, c),
            e => {
                AddError(e.ToString());
                
                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> GetCharacterByIdOrDefaultAsync(Request r, CancellationToken c) {
        var movie = await movieRepository.GetByIdOrDefaultAsync(r.Id, c);

        if (movie == null) return GetMovieError.MovieNotFound;

        var response = mapper.Map<MovieDto>(movie);
        
        return response;
    }
}