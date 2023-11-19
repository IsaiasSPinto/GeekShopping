using AutoMapper;
using GeekShopping.CuponAPI.Data.Dtos;
using GeekShopping.CuponAPI.Model;

namespace GeekShopping.CuponAPI.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new(cfg =>
        {
            cfg.CreateMap<Cupon, CuponDto>().ReverseMap();
        });
    }
}
