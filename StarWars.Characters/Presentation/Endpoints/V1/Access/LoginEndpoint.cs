using FastEndpoints;
using FastEndpoints.Security;
using OneOf;
using FluentValidation;
using StarWars.Characters.Models.Users;

namespace StarWars.Characters.Presentation.Endpoints.V1.Access;

using Result = OneOf<bool, LoginEndpoint.LoginError>;

public class LoginEndpoint(IUserRepository userRepository)
    : Endpoint<LoginEndpoint.LoginRequest, LoginEndpoint.LoginResposne> {
    #region Request/Response

    public class LoginRequest {
        public required string Login { get; init; }

        public required string Password { get; init; }
    }

    private class ReqValidator : Validator<LoginRequest> {
        public ReqValidator() {
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class LoginResposne {
        public string Login { get; init; }

        public string Token { get; init; }
    }

    #endregion

    public enum LoginError {
        IncorrectLogin,
        IncorrectPassword,
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Post("/access/login");
        Version(1);
        Validator<ReqValidator>();
    }

    public override Task HandleAsync(LoginRequest r, CancellationToken c) {
        var getUserResult = ValidateUserAsync(r, c);

        return getUserResult.Result.Match(
            _ => {
                var jwtToken = JwtBearer.CreateToken(
                    o => {
                        o.SigningKey = "Star wars characters super secret hope this enough";
                        o.ExpireAt = DateTime.UtcNow.AddDays(1);
                    }
                );
                var response = new LoginResposne {Login = r.Login, Token = jwtToken};
                
                return SendOkAsync(response, c);
            },
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> ValidateUserAsync(LoginRequest r, CancellationToken c) {
        var user = await userRepository.GetUserByLoginOrDefaultAsync(r.Login, c);

        if (user == null) return LoginError.IncorrectLogin;
        if (user.Password != r.Password) return LoginError.IncorrectPassword;

        return true;
    }
}