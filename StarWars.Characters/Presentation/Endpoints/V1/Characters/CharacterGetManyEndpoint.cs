using FastEndpoints;
using StarWars.Characters.Configuration.Utils.Filtering;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Presentation.Dtos;
using StarWars.Characters.Presentation.Utils.Paging;
using IMapper = AutoMapper.IMapper;

namespace StarWars.Characters.Presentation.Endpoints.V1.Characters;

public class CharacterGetManyEndpoint(
    ICharacterRepository characterRepository,
    IMapper mapper
) : Endpoint<CharacterGetManyEndpoint.Request, CharacterGetManyEndpoint.GetManyCharacterResponse> {
    #region Request/Response

    public class Request {
        [BindFrom("page")]
        public int Page { get; init; } = 1;
        
        [BindFrom("per_page")]
        public int PerPage { get; init; } = 5;
        
        [BindFrom("year_lower_bound")]
        public int? YearLowerBound { get; init; }
        
        [BindFrom("year_upper_bound")]
        public int? YearUpperBound { get; init; }
    
        [BindFrom("movie_id")]
        public ICollection<int>? MoviesIds { get; init; }
    
        [BindFrom("home_world_id")]
        public int? HomeWorldId { get; init; }
    
        [BindFrom("gender")]
        public CharacterGender? Gender { get; init; }
    }

    public class GetManyCharacterResponse {
        public required ICollection<CharacterDto> Items { get; init; }

        public required PageInfo PageInfo { get; init; }
    }

    #endregion
    
    public override void Configure() {
        AllowAnonymous();
        
        Get("/characters");
        Version(1);
        
        Summary(x => {
            x.Summary = "Получить всех персонажей";
        });
    }

    public override async Task HandleAsync(Request r, CancellationToken c) {
        var filteringParams = new CharacterFilteringParams(r.YearLowerBound, r.YearUpperBound, r.HomeWorldId, r.Gender, r.MoviesIds);
        var filtered = await characterRepository.GetManyFilteredAsync(filteringParams, c);
        
        var paginated = filtered.Skip((r.Page - 1) * r.PerPage).Take(r.PerPage).ToList();
        
        var response = new GetManyCharacterResponse {
            Items = mapper.Map<ICollection<CharacterDto>>(paginated),
            PageInfo = PageInfo.Create(filtered.Count, r.Page, r.PerPage)
        };
        
        await SendOkAsync(response, cancellation: c);
    }
}