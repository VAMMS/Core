using AutoMapper;
using VAMMS.Shared.Dtos;
using VAMMS.Shared.Models;

namespace VAMMS.Core.Utils;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<User, UserDto>();
    }
}
