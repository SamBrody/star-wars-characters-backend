using AutoMapper;
using StarWars.Characters.Models.Species;
using StarWars.Characters.Presentation.Dtos;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class SpeciesMappingProfile : Profile {
    public SpeciesMappingProfile() {
        CreateMap<Species, SpeciesDto>();
    }
}