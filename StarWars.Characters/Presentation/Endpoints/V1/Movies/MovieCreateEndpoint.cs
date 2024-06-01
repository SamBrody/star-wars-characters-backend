using FastEndpoints;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Movies;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Movies;

public class MovieCreateEndpoint(ISender sender, IMapper mapper) : Endpoint<MovieCreateEndpoint.CreateMovieRequest> {
    #region Request

    public class CreateMovieRequest {
        public required string Name { get; init; }
    }
    
    private class ReqValidator : Validator<CreateMovieRequest> {
        public ReqValidator() => RuleFor(x => x.Name).NotEmpty();
    }

    #endregion
    
    public override void Configure() {
        AllowAnonymous();

        Post("/movies");
        Version(1);
        Validator<ReqValidator>();

        Summary(x => {
            x.Summary = "Регистрация нового фильма";
        });
    }
    
    public override Task HandleAsync(CreateMovieRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterMovieCommand>(r);
        
        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}