using AutoMapper;
using StarWars.Characters.Application.Characters;
using StarWars.Characters.Models.Characters;
using StarWars.Characters.Presentation.Endpoints.V1.Characters;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class CharacterMappingProfile : Profile {
    public CharacterMappingProfile() {
        CreateMap<CharacterCreateRequest, RegisterCharacterCommand>();
        CreateMap<Character, CharacterGetManyResponse>()
            .ForMember(dest => dest.HomeWorld, opt => opt.MapFrom(src => src.HomeWorld))
            .ForMember(dest => dest.Species, opt => opt.MapFrom(src => src.Species))
            .ForMember(dest => dest.Movies, opt => opt.MapFrom(src => src.Movies));
    }
}