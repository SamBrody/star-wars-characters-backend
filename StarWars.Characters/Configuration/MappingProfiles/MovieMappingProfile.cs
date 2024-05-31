using AutoMapper;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Presentation.Endpoints.V1.Characters;
using StarWars.Characters.Presentation.Endpoints.V1.Movies;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class MovieMappingProfile : Profile {
    public MovieMappingProfile() {
        CreateMap<Movie, MovieGetManyResponse>();
        CreateMap<Movie, MovieDto>();
    }
}