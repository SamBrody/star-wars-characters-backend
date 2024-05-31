using AutoMapper;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Presentation.Dtos;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class PlanetMappingProfile : Profile {
    public PlanetMappingProfile() {
        CreateMap<Planet, PlanetDto>();
    }
}