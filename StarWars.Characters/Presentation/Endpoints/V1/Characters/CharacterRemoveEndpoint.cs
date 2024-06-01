using FastEndpoints;
using MediatR;
using StarWars.Characters.Application.Characters;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class CharacterRemoveEndpoint(ISender sender): Endpoint<CharacterRemoveEndpoint.Request> {
    public class Request {
        [BindFrom("id")]
        public int Id { get; init; }
    }
    
    public override void Configure() {
        AllowAnonymous();
        Delete("/characters/{id}");
    }

    public override Task HandleAsync(Request r, CancellationToken c) {
        return sender.Send(new RemoveCharacterCommand(r.Id), c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}