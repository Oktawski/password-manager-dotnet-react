using PasswordManager.Dtos;
using PasswordManager.Models;

namespace PasswordManager.Profiles;

public class PasswordsProfile : AutoMapper.Profile
{
    public PasswordsProfile()
    {
        CreateMap<Password, PasswordReadDto>()
            .ForMember(e => e.Application, t => t.MapFrom(map => map.ApplicationNormalized));
        CreateMap<PasswordCreateDto, Password>();
    }
}
