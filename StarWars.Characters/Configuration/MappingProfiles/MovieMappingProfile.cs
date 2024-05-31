using AutoMapper;
using StarWars.Characters.Models.Movies;
using StarWars.Characters.Presentation.Dtos;
using StarWars.Characters.Presentation.Endpoints.V1.Movies;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class MovieMappingProfile : Profile {
    public MovieMappingProfile() {
        CreateMap<Movie, MovieDto>();
    }
}