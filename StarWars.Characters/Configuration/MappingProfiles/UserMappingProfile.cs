using AutoMapper;
using StarWars.Characters.Models.Users;
using StarWars.Characters.Presentation.Dtos;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class UserMappingProfile : Profile {
    public UserMappingProfile() {
        CreateMap<User, UserDto>();
    }
}