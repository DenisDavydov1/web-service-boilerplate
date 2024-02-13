using AutoMapper;
using BoilerPlate.Data.Domain.Entities.System;
using BoilerPlate.Data.DTO.Common.Responses;
using BoilerPlate.Data.DTO.System.Users.Requests;
using BoilerPlate.Data.DTO.System.Users.Responses;

namespace BoilerPlate.App.Mappers.System;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<User, IdDto>();

        CreateMap<RegisterUserDto, User>()
            .ForMember(d => d.SecurityQuestions, o => o.Ignore());
    }
}