using AutoMapper;
using DigitalLibrary.BLL.Mapping;

namespace DigitalLibrary.Tests.Helpers;

public static class MapperHelper
{
    public static IMapper CreateMapper()
    {
        MapperConfiguration mapperConfiguration = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<BllMappingProfile>();
        });

        return mapperConfiguration.CreateMapper();
    }
}