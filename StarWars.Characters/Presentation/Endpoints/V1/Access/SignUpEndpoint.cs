using FastEndpoints;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Users;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Access;

public class SignUpEndpoint(ISender sender, IMapper mapper) : Endpoint<SignUpEndpoint.SignUpRequest> {
    #region Request

    public class SignUpRequest {
        public required string Login { get; init; }
        
        public required string Password { get; init; }
    }

    private class ReqValidator : Validator<SignUpRequest> {
        public ReqValidator() {
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    #endregion

    public override void Configure() {
        AllowAnonymous();
        
        Post("/access/sign-up");
        Version(1);
        Validator<ReqValidator>();

        Summary(x => {
            x.Summary = "Регистрация нового пользователя системы";
        });
    }

    public override Task HandleAsync(SignUpRequest r, CancellationToken c) {
        var cmd = mapper.Map<RegisterUserCommand>(r);

        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                if (e == RegisterUserError.LoginIsAlreadyTaken) AddError(r => r.Login, "Логин уже занят! Введите другой");

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}