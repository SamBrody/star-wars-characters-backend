using FastEndpoints;
using FastEndpoints.Security;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Characters;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class CharacterRemoveEndpoint(ISender sender): Endpoint<CharacterRemoveEndpoint.Request> {
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
    
    public override void Configure() {
        Delete("/characters/{id}");
        Version(1);
        Validator<ReqValidator>();
        
        Summary(s => {
            s.Summary = "Удаление Персонажа";
        });
    }

    public override Task HandleAsync(Request r, CancellationToken c) {
        var claimValue = HttpContext.User.ClaimValue("UserId");
        Int32.TryParse(claimValue, out var userId);
        
        return sender.Send(new RemoveCharacterCommand(r.Id, userId), c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                if (e == RemoveCharacterError.CharacterNotFound) AddError("Персонаж с таким индентификатором не найден");
                if (e == RemoveCharacterError.OnlyAuthorCanRemoveCharacter) AddError("Удалить персонажа может тот, кто его добавил");

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}