using AutoMapper;
using Example.Model;

namespace Example;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<CloudResource, CloudResourceResponse>();
    }
}