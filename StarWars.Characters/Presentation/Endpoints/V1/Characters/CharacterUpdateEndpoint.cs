using FastEndpoints;
using FluentValidation;
using MediatR;
using StarWars.Characters.Application.Characters;
using StarWars.Characters.Models.Characters;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

using Request = CharacterUpdateEndpoint.UpdateCharacterRequest;

public class CharacterUpdateEndpoint(ISender sender, IMapper mapper) : Endpoint<Request> {
    #region Request

    public class UpdateCharacterRequest {
        [BindFrom("id")]
        public int Id { get; init; }
    
        public string Name { get; init; }
        
        public string OriginalName { get; init; }
    
        public CharacterBirthDay BirthDay { get; init; }
    
        public int PlanetId { get; init; }
    
        public CharacterGender Gender { get; init; }
    
        public int SpeciesId { get; init; }
    
        public int Height { get; init; }
    
        public string HairColor { get; init; }
    
        public string EyeColor { get; init; }
    
        public string Description { get; init; }
    
        public ICollection<int> MovieIds { get; init; }
        
        public int UserId { get; init; }
    }
    
    private class ReqValidator : Validator<UpdateCharacterRequest> {
        public ReqValidator() {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Неверный идентификатор");
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.OriginalName).NotEmpty();
            RuleFor(x => x.BirthDay).NotEmpty();
            RuleFor(x => x.BirthDay.Year)
                .GreaterThan(0)
                .WithMessage("Год должен быть не отрицательным и больше 0")
                .NotEmpty();
            RuleFor(x => x.PlanetId).NotEmpty();
            RuleFor(x => x.Gender).NotNull();
            RuleFor(x => x.SpeciesId).NotEmpty();
            RuleFor(x => x.Height)
                .GreaterThan(0)
                .WithMessage("Рост должен быть не отрицательным и больше 0")
                .NotEmpty();
            RuleFor(x => x.HairColor).NotEmpty();
            RuleFor(x => x.EyeColor).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.MovieIds).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }

    #endregion
    
    public override void Configure() {
        Put("characters/{id}");
        Version(1);
        Validator<ReqValidator>();

        Summary(s => {
            s.Summary = "Обновление параметров Персонажа";
        });
    }

    public override Task HandleAsync(Request r, CancellationToken c) {
        var cmd = mapper.Map<UpdateCharacterCommand>(r);

        return sender.Send(cmd, c).Result.Match(
            _ => SendOkAsync(c),
            e => {
                AddError(e.ToString());

                return SendErrorsAsync(cancellation: c);
            }
        );
    }
}