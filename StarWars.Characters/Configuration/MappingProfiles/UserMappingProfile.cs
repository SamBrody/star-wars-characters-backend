using AutoMapper;
using StarWars.Characters.Application.Users;
using StarWars.Characters.Models.Users;
using StarWars.Characters.Presentation.Dtos;
using StarWars.Characters.Presentation.Endpoints.V1.Access;

namespace StarWars.Characters.Configuration.MappingProfiles;

public class UserMappingProfile : Profile {
    public UserMappingProfile() {
        CreateMap<User, UserDto>();
        CreateMap<SignUpEndpoint.SignUpRequest, RegisterUserCommand>();
    }
}