using AutoMapper;
using StarWars.Characters.Application.Species;
using StarWars.Characters.Models.Species;
using StarWars.Characters.Presentation.Dtos;
using StarWars.Characters.Presentation.Endpoints.V1.Species;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class SpeciesMappingProfile : Profile {
    public SpeciesMappingProfile() {
        CreateMap<Species, SpeciesDto>();
        CreateMap<SpeciesCreateEndpoint.CreateSpeciesRequest, RegisterSpeciesCommand>();
    }
}