using FastEndpoints;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Users;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Access;

public class RegisterUserEndpoint(ISender sender, IMapper mapper) : Endpoint<RegisterUserEndpoint.RegisterUserRequest> {
    #region Request

    public class RegisterUserRequest {
        public required string Login { get; init; }
        
        public required string Password { get; init; }
    }

    private class ReqValidator : Validator<RegisterUserRequest> {
        public ReqValidator() {
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    #endregion

    public override void Configure() {
        AllowAnonymous();
        
        Post("/users/register");
        Version(1);
        Validator<ReqValidator>();

        Summary(x => {
            x.Summary = "Регистрация нового пользователя системы";
        });
    }

    public override Task HandleAsync(RegisterUserRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterUserCommand>(r);

        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendOkAsync(cancellation: c);
            }
        );
    }
}