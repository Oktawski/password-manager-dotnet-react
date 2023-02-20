using PasswordManager.Models;

namespace PasswordManager.Profiles;

public class UsersProfile : AutoMapper.Profile
{
    public UsersProfile()
    {
        CreateMap<UserCreateDto, ApplicationUser>()
            .ForMember(e => e.Email, t => t.MapFrom(map => map.Email))
            .ForMember(e => e.UserName, t => t.MapFrom(map => map.Username))
            .ForMember(e => e.Id, t => Guid.NewGuid().ToString())
            .ForMember(e => e.PasswordHash, t => t.MapFrom(map => BCrypt.Net.BCrypt.HashPassword(map.Password)));
    }
}
