using FastEndpoints;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Characters;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class CharacterRemoveEndpoint(ISender sender): Endpoint<CharacterRemoveEndpoint.Request> {
    #region Request

    public class Request {
        [BindFrom("id")]
        public int Id { get; init; }
        
        public int UserId { get; init; }
    }

    private class ReqValidator : Validator<Request> {
        public ReqValidator() {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Неверный идентификатор");
            RuleFor(x => x.UserId).NotEmpty();
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
        return sender.Send(new RemoveCharacterCommand(r.Id, r.UserId), c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}