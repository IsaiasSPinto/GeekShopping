using AutoMapper;
using GeekShopping.ProductAPI.Data.Dtos;
using GeekShopping.ProductAPI.Model;

namespace GeekShopping.ProductAPI.Config;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        return new(cfg =>
        {
            cfg.CreateMap<Product, ProductDto>().ReverseMap();
        });
    }
}
