using MediatR;
using OneOf;
using StarWars.Characters.Models.Users;

namespace StarWars.Characters.Application.Users;

using Command = RegisterUserCommand;
using Result = OneOf<int, RegisterUserError>;

public record RegisterUserCommand(string Login, string Password) : IRequest<Result>, ITransactional;

internal class RegisterUserCommandHandler(IUserRepository userRepository) : IRequestHandler<Command, Result> {
    public async Task<Result> Handle(Command cmd, CancellationToken c) => await CreateUserAsync(cmd, c);

    private async Task<Result> CreateUserAsync(Command cmd, CancellationToken c) {
        var newUser = new User { Login = cmd.Login, Password = cmd.Password };

        return await userRepository.InsertAsync(newUser, c);
    }
}

public enum RegisterUserError { }