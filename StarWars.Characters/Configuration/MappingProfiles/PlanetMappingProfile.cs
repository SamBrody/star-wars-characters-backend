using AutoMapper;
using StarWars.Characters.Application.Planets;
using StarWars.Characters.Models.Planets;
using StarWars.Characters.Presentation.Dtos;
using StarWars.Characters.Presentation.Endpoints.V1.Planets;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class PlanetMappingProfile : Profile {
    public PlanetMappingProfile() {
        CreateMap<Planet, PlanetDto>();
        CreateMap<PlanetCreateEndpoint.CreatePlanetRequest, RegisterPlanetCommand>();
    }
}