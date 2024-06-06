using FastEndpoints;
using FastEndpoints.Security;
using OneOf;
using FluentValidation;
using StarWars.Characters.Models.Users;

namespace StarWars.Characters.Presentation.Endpoints.V1.Access;

using Result = OneOf<int, SignInEndpoint.SignInError>;

public class SignInEndpoint(IUserRepository userRepository)
    : Endpoint<SignInEndpoint.SignInRequest, SignInEndpoint.SignInResposne> {
    #region Request/Response

    public class SignInRequest {
        public required string Login { get; init; }

        public required string Password { get; init; }
    }

    private class ReqValidator : Validator<SignInRequest> {
        public ReqValidator() {
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }

    public class SignInResposne {
        public string Login { get; init; }

        public string Token { get; init; }
    }

    #endregion

    public enum SignInError {
        IncorrectLogin,
        IncorrectPassword,
    }
    
    public override void Configure() {
        AllowAnonymous();
        
        Post("/access/signin");
        Version(1);
        Validator<ReqValidator>();
    }

    public override Task HandleAsync(SignInRequest r, CancellationToken c) {
        var getUserResult = ValidateUserAsync(r, c);

        return getUserResult.Result.Match(
            id => {
                var jwtToken = JwtBearer.CreateToken(
                    o => {
                        o.SigningKey = "A_Secret_Token_Signing_Key_Longer_Than_32_Characters";
                        o.ExpireAt = DateTime.UtcNow.AddDays(1);
                        o.User.Claims.Add(("UserLogin", r.Login));
                        o.User["UserId"] = id.ToString();
                    }
                );
                var response = new SignInResposne {Login = r.Login, Token = jwtToken};
                
                return SendOkAsync(response, c);
            },
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }

    private async Task<Result> ValidateUserAsync(SignInRequest r, CancellationToken c) {
        var user = await userRepository.GetUserByLoginOrDefaultAsync(r.Login, c);

        if (user == null) return SignInError.IncorrectLogin;
        if (user.Password != r.Password) return SignInError.IncorrectPassword;

        return user.Id;
    }
}