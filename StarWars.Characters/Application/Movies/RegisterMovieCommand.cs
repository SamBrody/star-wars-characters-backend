using MediatR;
using OneOf;
using StarWars.Characters.Models.Movies;

namespace StarWars.Characters.Application.Movies;

using Command = RegisterMovieCommand;
using Result = OneOf<int, CreateMovieError>;

public record RegisterMovieCommand(string Name) : IRequest<Result>, ITransactional;

public sealed class RegisterMovieCommandHandler(IMovieRepository movieRepository) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) => await CreateMovieAsync(cmd, c);

    private async Task<Result> CreateMovieAsync(Command cmd, CancellationToken c) {
        var newMovie = new Movie { Name = cmd.Name, };
        
        return await movieRepository.InsertAsync(newMovie, c);
    }
}

public enum CreateMovieError { }