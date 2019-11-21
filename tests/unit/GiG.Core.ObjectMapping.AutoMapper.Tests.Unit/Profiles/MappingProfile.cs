using AutoMapper;
using GiG.Core.ObjectMapping.AutoMapper.Tests.Unit.Models;

namespace GiG.Core.ObjectMapping.AutoMapper.Tests.Unit.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Person, PersonEntity>().ReverseMap();
        }
    }
}
